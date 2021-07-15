using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;

[Serializable]
class SharableSpreadSheet
{
    private bool prints = false;

    public int rows;
    public int cols;
    public String[,] arrSheet;

    // mutex to write(set)
    private List<Mutex> writeMutexRows;
    private List<bool> boolWriteMutexRows;
    private List<Mutex> writeMutexCols;
    private List<bool> boolWriteMutexCols;

    private List<SemaphoreSlim> readColsSemaphore;
    private List<SemaphoreSlim> readRowsSemaphore;

    private SemaphoreSlim searchSemaphore;

    private int searchSemaphoreSize;
    private bool concurentSearch;

    private Mutex allSheetMutex;
    private bool boolAllSheet;

    private BinaryFormatter formatter;

    public SharableSpreadSheet(int nRows, int nCols)
    {
        // construct a nRows*nCols spreadsheet
        this.rows = nRows;
        this.cols = nCols;

        this.arrSheet = new String[nRows, nCols];
        for (int i = 0; i < nRows; i++)
        {
            for (int j = 0; j < nCols; j++)
            {
                this.arrSheet[i, j] = "";
            }
        }

        this.writeMutexRows = new List<Mutex>();
        this.writeMutexCols = new List<Mutex>();

        this.boolWriteMutexRows = new List<bool>();
        this.boolWriteMutexCols = new List<bool>();

        this.readRowsSemaphore = new List<SemaphoreSlim>();
        this.readColsSemaphore = new List<SemaphoreSlim>();

        for (int i = 0; i < nRows; i++)
        {
            this.writeMutexRows.Add(new Mutex());
            this.readRowsSemaphore.Add(new SemaphoreSlim(int.MaxValue));
            this.boolWriteMutexRows.Add(false);
        }
        for (int i = 0; i < nCols; i++)
        {
            this.writeMutexCols.Add(new Mutex());
            this.readColsSemaphore.Add(new SemaphoreSlim(int.MaxValue));
            this.boolWriteMutexCols.Add(false);
        }

        this.concurentSearch = false;
        this.searchSemaphoreSize = -1;

        this.searchSemaphore = new SemaphoreSlim(int.MaxValue);
        this.formatter = new BinaryFormatter();

        this.allSheetMutex = new Mutex();
        this.boolAllSheet = false;
    }


    private void setSheet(SaveSheet saveSheet)
    {
        this.rows = saveSheet.rows;
        this.cols = saveSheet.cols;
        this.arrSheet = saveSheet.arrSheet;

        this.concurentSearch = saveSheet.concurentSearch;
        this.searchSemaphoreSize = saveSheet.searchSemaphoreSize;

        this.writeMutexRows = new List<Mutex>();
        this.writeMutexCols = new List<Mutex>();

        this.boolWriteMutexRows = new List<bool>();
        this.boolWriteMutexCols = new List<bool>();

        this.readRowsSemaphore = new List<SemaphoreSlim>();
        this.readColsSemaphore = new List<SemaphoreSlim>();

        if (this.searchSemaphoreSize != -1)
        {
            this.searchSemaphore = new SemaphoreSlim(this.searchSemaphoreSize);
        }
        else
        {
            this.searchSemaphore = new SemaphoreSlim(int.MaxValue);
        }


        for (int i = 0; i < this.rows; i++)
        {
            this.writeMutexRows.Add(new Mutex());
            this.boolWriteMutexRows.Add(false);
            this.readRowsSemaphore.Add(new SemaphoreSlim(int.MaxValue));
        }
        for (int i = 0; i < this.cols; i++)
        {
            this.writeMutexCols.Add(new Mutex());
            this.boolWriteMutexCols.Add(false);
            this.readColsSemaphore.Add(new SemaphoreSlim(int.MaxValue));
        }

        this.formatter = new BinaryFormatter();

        this.allSheetMutex = new Mutex();
        this.boolAllSheet = false;
    }

    public String getCell(int row, int col)
    {
        // return the string at [row,col]

        // wait that if someone write to the spesicif cell will finish to write
        while (this.boolWriteMutexRows[row] && this.boolWriteMutexCols[col] && this.boolAllSheet) { }

        // reduce the neccesary readers semaphores by one
        this.readColsSemaphore[col].Wait();
        this.readRowsSemaphore[row].Wait();

        //get (read) the cell from the spreadsheet
        String cellStr = this.arrSheet[row, col];

        // increament the semaphores by one (finish reading)
        this.readColsSemaphore[col].Release();
        this.readRowsSemaphore[row].Release();

        // return the cell value
        return cellStr;
    }

    public bool setCell(int row, int col, String str)
    {
        // set the string at [row,col]

        // check if can write
        try
        {
            // get the write mutex of the col and row we are going to write in
            this.writeMutexCols[col].WaitOne();
            this.boolWriteMutexCols[col] = true;
            this.writeMutexRows[row].WaitOne();
            this.boolWriteMutexRows[row] = true;

            // wait that all readers that already read before locking the write mutexes will finish to read
            while (this.readColsSemaphore[col].CurrentCount != int.MaxValue && this.readRowsSemaphore[row].CurrentCount != int.MaxValue && this.boolAllSheet) { }

            // set the new string in the spreadsheet
            this.arrSheet[row, col] = str;

            //release the write mutexes
            this.writeMutexCols[col].ReleaseMutex();
            this.boolWriteMutexCols[col] = false;
            this.writeMutexRows[row].ReleaseMutex();
            this.boolWriteMutexRows[row] = false;
            return true;
        }
        catch
        {
            this.boolWriteMutexCols[col] = false;
            this.boolWriteMutexRows[row] = false;
            return false;
        }
    }

    public bool searchString(String str, ref int row, ref int col)
    {
        // search the cell with string str, and return true/false accordingly.
        // stores the location in row,col.
        // return the first cell that contains the string (search from first row to the last row)
        try
        {
            this.searchSemaphore.Wait();
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    if (str.Equals(this.getCell(i, j)))
                    {
                        this.searchSemaphore.Release();
                        return true;
                    }
                }
            }
            this.searchSemaphore.Release();
            return false;
        }
        catch
        {
            try
            {
                this.searchSemaphore.Release();
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }

    public bool exchangeRows(int row1, int row2)
    {
        // exchange the content of row1 and row2
        try
        {
            while (this.boolWriteMutexRows[row1] && this.boolWriteMutexRows[row2] && this.boolAllSheet) { }
            this.writeMutexRows[row1].WaitOne();
            this.boolWriteMutexRows[row1] = true;
            this.writeMutexRows[row2].WaitOne();
            this.boolWriteMutexRows[row2] = true;
            String temp;
            for (int i = 0; i < this.cols; i++)
            {
                temp = this.arrSheet[row1, i];
                this.arrSheet[row1, i] = this.arrSheet[row2, i];
                this.arrSheet[row2, i] = temp;
            }
            this.writeMutexRows[row1].ReleaseMutex();
            this.boolWriteMutexRows[row1] = false;
            this.writeMutexRows[row2].ReleaseMutex();
            this.boolWriteMutexRows[row2] = false;
            return true;
        }
        catch
        {
            this.boolWriteMutexRows[row1] = false;
            this.boolWriteMutexRows[row2] = false;
            return false;
        }

    }

    public bool exchangeCols(int col1, int col2)
    {
        // exchange the content of col1 and col2

        try
        {
            while (this.boolWriteMutexRows[col1] && this.boolWriteMutexRows[col2] && this.boolAllSheet) { }
            this.writeMutexCols[col1].WaitOne();
            this.boolWriteMutexCols[col1] = true;
            this.writeMutexCols[col2].WaitOne();
            this.boolWriteMutexCols[col2] = true;
            String temp;
            for (int i = 0; i < this.cols; i++)
            {
                temp = this.arrSheet[i, col1];
                this.arrSheet[i, col1] = this.arrSheet[i, col2];
                this.arrSheet[i, col2] = temp;
            }
            this.writeMutexCols[col1].ReleaseMutex();
            this.boolWriteMutexCols[col1] = false;
            this.writeMutexCols[col2].ReleaseMutex();
            this.boolWriteMutexCols[col2] = false;
            return true;
        }
        catch
        {
            this.boolWriteMutexCols[col1] = false;
            this.boolWriteMutexCols[col2] = false;
            return false;
        }

    }

    public bool searchInRow(int row, String str, ref int col)
    {
        // perform search in specific row
        try
        {
            this.searchSemaphore.Wait();
            for (int j = 0; j < this.cols; j++)
            {
                if (str.Equals(this.getCell(row, j)))
                {
                    col = j;
                    return true;
                }
            }
            this.searchSemaphore.Release();
            return false;
        }
        catch
        {
            try
            {
                this.searchSemaphore.Release();
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }

    public bool searchInCol(int col, String str, ref int row)
    {
        // perform search in specific col
        try
        {
            this.searchSemaphore.Wait();
            for (int i = 0; i < this.cols; i++)
            {
                if (str.Equals(this.getCell(i, col)))
                {
                    row = i;
                    return true;
                }
            }
            this.searchSemaphore.Release();
            return false;
        }
        catch
        {
            try
            {
                this.searchSemaphore.Release();
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }

    public bool searchInRange(int col1, int col2, int row1, int row2, String str, ref int row, ref int col)
    {
        // perform search within spesific range: [row1:row2,col1:col2] 
        //includes col1,col2,row1,row2
        try
        {
            this.searchSemaphore.Wait();
            for (int i = row1; i < row2; i++)
            {
                for (int j = col1; j < col2; j++)
                {
                    if (str.Equals(this.getCell(i, j)))
                    {
                        row = i;
                        col = j;
                        this.searchSemaphore.Release();
                        return true;
                    }
                }
            }
            this.searchSemaphore.Release();
            return false;
        }
        catch
        {
            try
            {
                this.searchSemaphore.Release();
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }

    public bool addRow(int row1)
    {
        //add a row after row1
        if (this.prints)
        {
            Console.WriteLine("[addRow] enter method");
        }
        this.allSheetMutex.WaitOne();
        this.boolAllSheet = true;
        while (!this.boolWriteMutexCols.Contains(false) && !this.boolWriteMutexRows.Contains(false))
        {
            if (this.prints)
            {
                Console.WriteLine("[addRow] waiting");
            }
        }

        if (!this.lockSpreadSheet())
        {
            this.boolAllSheet = false;
            this.allSheetMutex.ReleaseMutex();
            return false;
        }
        String[,] newSheet = new string[this.rows + 1, this.cols];

        this.boolWriteMutexRows.Add(true);
        Mutex newMutex = new Mutex();
        newMutex.WaitOne();
        this.writeMutexRows.Add(newMutex);
        SemaphoreSlim newSemaphore = new SemaphoreSlim(int.MaxValue);
        this.readRowsSemaphore.Add(newSemaphore);


        for (int i = 0; i < this.rows; i++)
        {
            if (i < row1)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    if (this.prints)
                    {
                        Console.WriteLine("[addRow] copy cell [{0},{1}]", i, j);
                    }
                    newSheet[i, j] = this.arrSheet[i, j];
                }
            }
            else if (i == row1)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    if (this.prints)
                    {
                        Console.WriteLine("[addRow] add blank cell [{0},{1}]", i, j);
                    }
                    newSheet[i, j] = "";
                }
            }
            else
            {
                for (int j = 0; j < this.cols; j++)
                {
                    if (this.prints)
                    {
                        Console.WriteLine("[addRow] move cell [{0},{1}] to cell [{2},{3}]", i, j, i + 1, j);
                    }
                    newSheet[i + 1, j] = this.arrSheet[i, j];
                }
            }

        }

        this.arrSheet = newSheet;

        this.rows += 1;

        if (!this.unlockSpreadSheet())
        {
            this.boolAllSheet = false;
            this.allSheetMutex.ReleaseMutex();
            return false;
        }
        this.boolAllSheet = false;
        this.allSheetMutex.ReleaseMutex();
        if (this.prints)
        {
            Console.WriteLine("[addRow] exit method");
        }
        return true;
    }

    public bool addCol(int col1)
    {
        if (this.prints)
        {
            Console.WriteLine("[addCol] enter method");
        }
        //add a column after col1
        this.allSheetMutex.WaitOne();
        this.boolAllSheet = true;
        while (!this.boolWriteMutexCols.Contains(false) && !this.boolWriteMutexRows.Contains(false))
        {
            if (this.prints)
            {
                Console.WriteLine("[addCol] waiting");
            }
        }
        if (!this.lockSpreadSheet())
        {
            this.boolAllSheet = false;

            this.allSheetMutex.ReleaseMutex();
            return false;
        }

        String[,] newSheet = new string[this.rows, this.cols + 1];

        this.boolWriteMutexCols.Add(true);
        Mutex newMutex = new Mutex();
        newMutex.WaitOne();
        this.writeMutexCols.Add(newMutex);
        SemaphoreSlim newSemaphore = new SemaphoreSlim(int.MaxValue);
        this.readColsSemaphore.Add(newSemaphore);


        for (int i = 0; i < this.cols; i++)
        {
            if (i < col1)
            {
                for (int j = 0; j < this.rows; j++)
                {
                    newSheet[j, i] = this.arrSheet[j, i];
                }
            }
            else if (i == col1)
            {
                for (int j = 0; j < this.rows; j++)
                {
                    newSheet[j, i] = "";
                }
            }
            else
            {
                for (int j = 0; j < this.rows; j++)
                {
                    newSheet[j, i + 1] = this.arrSheet[j, i];
                }
            }

        }

        this.arrSheet = newSheet;
        this.cols += 1;

        this.boolAllSheet = false;
        this.allSheetMutex.ReleaseMutex();
        if (!this.unlockSpreadSheet())
        {
            this.boolAllSheet = false;
            this.allSheetMutex.ReleaseMutex();
            if (this.prints)
            {
                Console.WriteLine("[addCol] failed unlock");
            }
            return false;
        }
        if (this.prints)
        {
            Console.WriteLine("[addCol] exit method");
        }
        return true;
    }

    public bool setConcurrentSearchLimit(int nUsers)
    {
        // this function aims to limit the number of users that can perform the search operations concurrently.
        // The default is no limit. When the function is called, the max number of concurrent search operations is set to nUsers. 
        // In this case additional search operations will wait for existing search to finish.

        this.searchSemaphoreSize = nUsers;
        this.searchSemaphore = new SemaphoreSlim(nUsers);
        this.concurentSearch = true;

        return true;
    }

    public void getSize(ref int nRows, ref int nCols)
    {
        // return the size of the spreadsheet in nRows, nCols
        nRows = this.rows;
        nCols = this.cols;
    }

    public bool save(String fileName)
    {
        // save the spreadsheet to a file fileName.
        // you can decide the format you save the data. There are several options.
        this.allSheetMutex.WaitOne();
        this.boolAllSheet = true;
        if (!this.lockSpreadSheet())
        {
            this.boolAllSheet = false;
            this.allSheetMutex.ReleaseMutex();
            return false;
        }
        SaveSheet saveSheet = new SaveSheet(this);
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        try
        {
            FileStream writerFileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            //SharableSpreadSheet saveSpreedSheet = new SharableSpreadSheet(this);
            this.formatter.Serialize(writerFileStream, saveSheet);
            //this.formatter.Serialize(writerFileStream, this.cols);

            writerFileStream.Close();
            if (this.unlockSpreadSheet())
            {
                this.boolAllSheet = false;
                this.allSheetMutex.ReleaseMutex();
                return true;
            }
            else
            {
                this.boolAllSheet = false;
                this.allSheetMutex.ReleaseMutex();
                return false;
            }
        }
        catch (Exception)
        {

            this.unlockSpreadSheet();
            this.boolAllSheet = false;
            this.allSheetMutex.ReleaseMutex();
            return false;
        } // end try-catch

    }

    public bool load(String fileName)
    {
        // load the spreadsheet from fileName
        // replace the data and size of the current spreadsheet with the loaded data
        this.allSheetMutex.WaitOne();
        this.boolAllSheet = true;
        if (!this.lockSpreadSheet())
        {
            this.boolAllSheet = false;
            this.allSheetMutex.ReleaseMutex();
            return false;
        }

        if (File.Exists(fileName))
        {
            try
            {
                FileStream readerFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                SaveSheet newSheet = (SaveSheet)this.formatter.Deserialize(readerFileStream);
                setSheet(newSheet);
                readerFileStream.Close();

                if (this.unlockSpreadSheet())
                {
                    this.boolAllSheet = false;
                    this.allSheetMutex.ReleaseMutex();
                    return true;
                }
                else
                {
                    this.boolAllSheet = false;
                    this.allSheetMutex.ReleaseMutex();
                    return false;
                }
            }
            catch (Exception)
            {
                this.unlockSpreadSheet();
                this.boolAllSheet = false;
                try
                {
                    this.allSheetMutex.ReleaseMutex();
                }
                catch(Exception)
                {
                    return false;
                }
                return false;
            }
        }
        return false;

    }

    private bool lockSpreadSheet()
    {
        try
        {
            for (int i = 0; i < this.writeMutexCols.Count; i++)
            {
                this.writeMutexCols[i].WaitOne();
                this.boolWriteMutexCols[i] = true;
            }
            for (int i = 0; i < this.writeMutexRows.Count; i++)
            {
                this.writeMutexRows[i].WaitOne();
                this.boolWriteMutexRows[i] = true;
            }
            return true;
        }
        catch (Exception)
        {
            for (int i = 0; i < this.writeMutexCols.Count; i++)
            {
                try
                {
                    this.writeMutexCols[i].ReleaseMutex();
                    this.boolWriteMutexCols[i] = false;
                }
                catch (Exception)
                {
                    this.boolWriteMutexCols[i] = false;
                }
            }
            for (int i = 0; i < this.writeMutexRows.Count; i++)
            {
                try
                {
                    this.writeMutexRows[i].ReleaseMutex();
                    this.boolWriteMutexRows[i] = false;

                }
                catch (Exception)
                {
                    this.boolWriteMutexRows[i] = false;
                }
            }
            return false;
        }
    }

    private bool unlockSpreadSheet()
    {
        try
        {
            for (int i = 0; i < this.writeMutexCols.Count; i++)
            {
                try
                {
                    this.writeMutexCols[i].ReleaseMutex();
                    this.boolWriteMutexCols[i] = false;
                }
                catch (Exception)
                {
                    this.boolWriteMutexCols[i] = false;
                }
            }
            for (int i = 0; i < this.writeMutexRows.Count; i++)
            {
                try
                {
                    this.writeMutexRows[i].ReleaseMutex();
                    this.boolWriteMutexRows[i] = false;
                }
                catch (Exception)
                {
                    this.boolWriteMutexRows[i] = false;
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    [Serializable]
    public class SaveSheet
    {
        public int rows;
        public int cols;
        public String[,] arrSheet;

        public bool concurentSearch;
        public int searchSemaphoreSize;
        public SaveSheet(SharableSpreadSheet sharableSpreadSheet)
        {
            sharableSpreadSheet.getSize(ref this.rows, ref this.cols);
            this.arrSheet = sharableSpreadSheet.arrSheet;
            this.concurentSearch = sharableSpreadSheet.concurentSearch;
            this.searchSemaphoreSize = sharableSpreadSheet.searchSemaphoreSize;
        }
    }
}