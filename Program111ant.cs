using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp10
{
    public class NcFile
    {
        public string FileName;
        public int FileSize;
        public string ModDate;
        public string ModTime;
        public bool IsFolder;

        public NcFile(string fileName, int fileSize, string modDate, string modTime, bool isFolder)
        {
            FileName = fileName;
            FileSize = fileSize;
            ModDate = modDate;
            ModTime = modTime;
            IsFolder = isFolder;
        }
    }

    class Program
    {

        const int MinW = 40;
        const int MinH = 10;

        static int W = 80;
        static int H = 25;
        static int Half = 40;
        static int TopY = 1;
        static int BoxH = 22;
        static int Rows = 17;
        static int LeftCols = 3;

        const char HLine = '\u2550';
        const char VLine = '\u2551';
        const char UL = '\u2554';
        const char UR = '\u2557';
        const char DL = '\u255A';
        const char DR = '\u255D';
        const char LT = '\u2560';
        const char RT = '\u2563';
        const char Col = '\u2502';

        static void Main()
        {
            PrepareWindow();

            List<NcFile> all = MakeFileList();

            List<NcFile> leftList = new List<NcFile>(all);
            List<NcFile> rightList = new List<NcFile>(all);

            leftList.Sort(SortByExt);
            rightList.Sort(SortByName);

            bool running = true;
            int lastW = -1, lastH = -1;

            while (running)
            {
                
                UpdateLayout();


                if (W != lastW || H != lastH)
                {
                    SafeClear();
                    lastW = W;
                    lastH = H;
                }

                if (W < MinW || H < MinH)
                {
                    PaintTooSmall();
                }
                else
                {
                    PaintMenu();
                    PaintLeft(leftList);
                    PaintRight(rightList);
                    PaintPrompt();
                    PaintFKeys();

                    SafeSetCursor(6, H - 2);
                    Console.CursorVisible = true;
                }


                if (Console.KeyAvailable)
                {
                    Console.CursorVisible = false;
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.F10)
                        running = false;
                }
                else
                {
                    Thread.Sleep(80);
                }
            }

            SafeClear();
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        
        static void UpdateLayout()
        {
            int cw, ch;
            try
            {
                cw = Console.WindowWidth;
                ch = Console.WindowHeight;
            }
            catch
            {
               
                cw = W;
                ch = H;
            }

            if (cw < 1) cw = 1;
            if (ch < 1) ch = 1;

            W = cw;
            H = ch;

            TopY = 1;
            
            BoxH = Math.Max(0, H - 3);
            Half = W / 2;

            
            LeftCols = Math.Max(1, (Half - 1) / 13);
            if (LeftCols > 3) LeftCols = 3;

            
            Rows = Math.Max(0, BoxH - 5);
        }

        static void SafeClear()
        {
            try { Console.Clear(); }
            catch {  }
        }

        static void SafeSetCursor(int x, int y)
        {
            try
            {
                if (x >= 0 && y >= 0 && x < Console.BufferWidth && y < Console.BufferHeight)
                    Console.SetCursorPosition(x, y);
            }
            catch { }
        }

        static void PaintTooSmall()
        {
            try
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;

                string msg = "Увеличьте окно консоли...";
                if (msg.Length > W) msg = msg.Substring(0, W);

                int y = Math.Max(0, H / 2);
                int x = Math.Max(0, (W - msg.Length) / 2);

                SafeSetCursor(x, y);
                Console.Write(msg);
            }
            catch { }
        }

        static void PrepareWindow()
        {
            Console.Title = "Norton Commander";
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            try
            {
                Console.SetWindowSize(80, 25);
                Console.SetBufferSize(80, 25);
            }
            catch
            {
                
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            SafeClear();
        }

        static List<NcFile> MakeFileList()
        {
            List<NcFile> list = new List<NcFile>();
            string[] lines =
            {
                "..;0;11.10.02;19:48;1",
                "Ajaccgdo;417392;12.10.02;9:02;0",
                "nc.cfg;2840;11.10.02;19:48;0",
                "nc_exit.com;128;25.05.95;5:00;0",
                "telemax.dat;8192;25.05.95;5:00;0",
                "nc_exit.doc;2048;25.05.95;5:00;0",
                "123view.exe;128380;25.05.95;5:00;0",
                "arcview.exe;81738;25.05.95;5:00;0",
                "bitmap.exe;54805;25.05.95;5:00;0",
                "clp2dib.exe;41914;25.05.95;5:00;0",
                "dbview.exe;110272;25.05.95;5:00;0",
                "draw2wmf.exe;49957;25.05.95;5:00;0",
                "drw2wmf.exe;64085;25.05.95;5:00;0",
                "ico2dib.exe;37925;25.05.95;5:00;0",
                "msp2dib.exe;27269;25.05.95;5:00;0",
                "nc.exe;102544;25.05.95;5:00;0",
                "ncclean.exe;34560;25.05.95;5:00;0",
                "ncdd.exe;45216;25.05.95;5:00;0",
                "ncedit.exe;89120;25.05.95;5:00;0",
                "ncff.exe;67344;25.05.95;5:00;0",
                "nclabel.exe;34112;25.05.95;5:00;0",
                "ncmain.exe;198656;25.05.95;5:00;0",
                "ncnet.exe;51200;25.05.95;5:00;0",
                "ncsf.exe;22016;25.05.95;5:00;0",
                "ncsi.exe;18432;25.05.95;5:00;0",
                "nczip.exe;84164;25.05.95;5:00;0",
                "packer.exe;84164;25.05.95;5:00;0",
                "paraview.exe;62116;25.05.95;5:00;0",
                "pct2dib.exe;38085;25.05.95;5:00;0",
                "playwave.exe;16384;25.05.95;5:00;0",
                "q&aview.exe;108694;25.05.95;5:00;0",
                "rbview.exe;67486;25.05.95;5:00;0",
                "refview.exe;126608;25.05.95;5:00;0",
                "saver.exe;34560;25.05.95;5:00;0",
                "telemax.exe;45056;25.05.95;5:00;0",
                "tif2dib.exe;47061;25.05.95;5:00;0",
                "vector.exe;63845;25.05.95;5:00;0",
                "wpb2dib.exe;38069;25.05.95;5:00;0",
                "wpv2wmf.exe;61349;25.05.95;5:00;0",
                "wpview.exe;105834;25.05.95;5:00;0",
                "nc.ext;512;25.05.95;5:00;0",
                "nc.fil;256;25.05.95;5:00;0",
                "ncpscrip.hdr;4192;25.05.95;5:00;0",
                "nc.hlp;156112;25.05.95;5:00;0",
                "ncff.hlp;8192;25.05.95;5:00;0",
                "telemax.hlp;4096;25.05.95;5:00;0",
                "nc.ico;766;25.05.95;5:00;0",
                "nc.ini;1024;11.10.02;19:48;0",
                "ncclean.ini;512;25.05.95;5:00;0",
                "norton.ini;640;25.05.95;5:00;0",
                "telemax.ini;768;25.05.95;5:00;0",
                "4372ansi.set;255;25.05.95;5:00;0",
                "8502ansi.set;255;25.05.95;5:00;0",
                "8632ansi.set;255;25.05.95;5:00;0",
                "8652ansi.set;255;25.05.95;5:00;0",
                "8662ansi.set;255;25.05.95;5:00;0",
                "ansi2437.set;255;25.05.95;5:00;0",
                "ansi2850.set;255;25.05.95;5:00;0",
                "ansi2863.set;255;25.05.95;5:00;0",
                "ansi2865.set;255;25.05.95;5:00;0",
                "ansi2866.set;255;25.05.95;5:00;0",
                "bungee.nss;16133;25.05.95;5:00;0",
                "MyFileWithLongName.txt;2048;11.10.02;19:48;0"
            };

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                bool folder = parts[4] == "1";
                list.Add(new NcFile(
                    parts[0],
                    Convert.ToInt32(parts[1]),
                    parts[2],
                    parts[3],
                    folder));
            }

            return list;
        }

        static string OnlyName(string full)
        {
            int p = full.LastIndexOf('.');
            if (p > 0)
                return full.Substring(0, p);
            return full;
        }

        static string OnlyExt(string full)
        {
            int p = full.LastIndexOf('.');
            if (p > 0 && p < full.Length - 1)
                return full.Substring(p + 1);
            return "";
        }

        static int SortByName(NcFile x, NcFile y)
        {
            if (x.FileName == ".." && y.FileName != "..") return -1;
            if (y.FileName == ".." && x.FileName != "..") return 1;
            return string.Compare(x.FileName, y.FileName, true);
        }

        static int SortByExt(NcFile x, NcFile y)
        {
            if (x.FileName == ".." && y.FileName != "..") return -1;
            if (y.FileName == ".." && x.FileName != "..") return 1;

            int c = string.Compare(OnlyExt(x.FileName), OnlyExt(y.FileName), true);
            if (c != 0)
                return c;
            return string.Compare(OnlyName(x.FileName), OnlyName(y.FileName), true);
        }

        static string MakeDosName(string full)
        {
            if (full == "..")
                return FitLeft("..", 12);

            string nm = OnlyName(full);
            string ex = OnlyExt(full);

            if (nm.Length > 8)
                nm = nm.Substring(0, 7) + "~";
            if (ex.Length > 3)
                ex = ex.Substring(0, 3);

            return FitLeft(nm, 8) + " " + FitLeft(ex, 3);
        }

        static ConsoleColor ColorOf(NcFile f)
        {
            if (f.IsFolder)
                return ConsoleColor.Yellow;
            return ConsoleColor.Cyan;
        }

        static void PaintMenu()
        {
            Put(0, 0, new string(' ', W), ConsoleColor.Black, ConsoleColor.Cyan);

            string[] menu = { "Левая", "Файл", "Диск", "Команды", "Правая" };
            int pos = 1;
            for (int k = 0; k < menu.Length; k++)
            {
                Put(pos, 0, menu[k].Substring(0, 1), ConsoleColor.Yellow, ConsoleColor.Cyan);
                Put(pos + 1, 0, menu[k].Substring(1), ConsoleColor.Black, ConsoleColor.Cyan);
                pos = pos + menu[k].Length + 3;
            }

            string clock = " 8 30";
            Put(Math.Max(0, W - clock.Length), 0, clock, ConsoleColor.Black, ConsoleColor.Cyan);
        }

        static void PaintLeft(List<NcFile> files)
        {
            int x0 = 0;
            int w = Half;
            FillArea(x0, TopY, w, BoxH, ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            PaintBox(x0, TopY, w, BoxH);
            PaintCaption(x0, TopY, w, @"C:\NC", false);

           
            if (BoxH < 6 || Rows < 1 || w < 6)
                return;

            PaintSplit(x0, TopY + BoxH - 3, w);

            int colW = Math.Max(4, (w - 2) / LeftCols);
            int nameWidth = Math.Max(3, colW - 1);

            StringBuilder head = new StringBuilder();
            head.Append(FitLeft("C:\u2193 Имя", nameWidth));
            for (int c = 1; c < LeftCols; c++)
                head.Append(Col).Append(FitLeft("Имя", nameWidth));
            Put(x0 + 1, TopY + 1, head.ToString(), ConsoleColor.Yellow, ConsoleColor.DarkBlue);

            int maxShow = Rows * LeftCols;
            int count = Math.Min(files.Count, maxShow);

            for (int c = 0; c < LeftCols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    int yy = TopY + 2 + r;

                    if (c < LeftCols - 1)
                        Put(x0 + 1 + (c + 1) * colW - 1, yy, Col.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);

                    int idx = c * Rows + r;
                    if (idx >= count)
                        continue;

                    NcFile f = files[idx];
                    Put(x0 + 1 + c * colW, yy, FitLeft(MakeDosName(f.FileName), nameWidth), ColorOf(f), ConsoleColor.DarkBlue);
                }
            }

            if (files.Count > 0)
                PaintStatus(x0 + 1, TopY + BoxH - 2, files[0], w - 2);
        }

        static void PaintRight(List<NcFile> files)
        {
            int x0 = Half;
            int w = W - Half;
            FillArea(x0, TopY, w, BoxH, ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            PaintBox(x0, TopY, w, BoxH);
            PaintCaption(x0, TopY, w, @"C:\NC", true);

            if (BoxH < 6 || Rows < 1 || w < 6)
                return;

            PaintSplit(x0, TopY + BoxH - 3, w);

            
            int avail = Math.Max(0, w - 2);

           
            var cols = new List<(string title, int width)>();
            cols.Add(("C:\u2193 Имя", Math.Min(12, avail)));

            if (avail >= 12 + 1 + 9) cols.Add(("Размер", 9));
            if (avail >= 12 + 1 + 9 + 1 + 8) cols.Add(("  Дата", 8));
            if (avail >= 12 + 1 + 9 + 1 + 8 + 1 + 6) cols.Add((" Время", 6));

            int[] xOff = new int[cols.Count];
            int cx = 0;
            for (int i = 0; i < cols.Count; i++)
            {
                xOff[i] = cx;
                cx += cols[i].width + 1;
            }

            for (int i = 0; i < cols.Count; i++)
            {
                Put(x0 + 1 + xOff[i], TopY + 1, FitLeft(cols[i].title, cols[i].width), ConsoleColor.Yellow, ConsoleColor.DarkBlue);
                if (i < cols.Count - 1)
                    Put(x0 + 1 + xOff[i] + cols[i].width, TopY + 1, Col.ToString(), ConsoleColor.Yellow, ConsoleColor.DarkBlue);
            }

            int count = Math.Min(files.Count, Rows);
            for (int i = 0; i < count; i++)
                PaintRow(x0 + 1, TopY + 2 + i, files[i], i == 0, cols, xOff);

            if (files.Count > 0)
                PaintStatus(x0 + 1, TopY + BoxH - 2, files[0], avail);
        }

        static void PaintRow(int x, int y, NcFile f, bool selected, List<(string title, int width)> cols, int[] xOff)
        {
            ConsoleColor back = selected ? ConsoleColor.Cyan : ConsoleColor.DarkBlue;
            ConsoleColor front = selected ? ConsoleColor.Black : ColorOf(f);
            ConsoleColor line = selected ? ConsoleColor.Black : ConsoleColor.Cyan;
            ConsoleColor text = selected ? ConsoleColor.Black : ConsoleColor.Cyan;

            for (int i = 0; i < cols.Count; i++)
            {
                string val;
                ConsoleColor col;

                switch (i)
                {
                    case 0:
                        val = FitLeft(MakeDosName(f.FileName), cols[i].width);
                        col = front;
                        break;
                    case 1:
                        val = FitRight(f.IsFolder ? "\u25BAКАТАЛОГ\u25C4" : f.FileSize.ToString(), cols[i].width);
                        col = selected ? ConsoleColor.Black : (f.IsFolder ? ConsoleColor.Cyan : front);
                        break;
                    case 2:
                        val = FitLeft(f.ModDate, cols[i].width);
                        col = text;
                        break;
                    default:
                        val = FitRight(f.ModTime, cols[i].width);
                        col = text;
                        break;
                }

                Put(x + xOff[i], y, val, col, back);

                if (i < cols.Count - 1)
                    Put(x + xOff[i] + cols[i].width, y, Col.ToString(), line, back);
            }
        }

        static void PaintStatus(int x, int y, NcFile f, int width)
        {
            if (width <= 0) return;

            Put(x, y, new string(' ', width), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            Put(x, y, FitLeft(f.FileName, Math.Min(12, width)), ConsoleColor.Yellow, ConsoleColor.DarkBlue);

            string mid = f.IsFolder ? "\u25BAКАТАЛОГ\u25C4" : f.FileSize.ToString();

            if (width > 12)
                Put(x + 12, y, FitRight(mid, Math.Min(9, width - 12)), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            if (width > 22)
                Put(x + 22, y, " " + FitLeft(f.ModDate, Math.Min(8, width - 23)), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            if (width > 31)
                Put(x + 31, y, " " + FitRight(f.ModTime, Math.Min(5, width - 32)), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
        }

        static void PaintPrompt()
        {
            int y = H - 2;
            Put(0, y, new string(' ', W), ConsoleColor.Gray, ConsoleColor.Black);
            Put(0, y, @"C:\NC>", ConsoleColor.Gray, ConsoleColor.Black);
        }

        static void PaintFKeys()
        {
            int y = H - 1;
            Put(0, y, new string(' ', W), ConsoleColor.White, ConsoleColor.Black);

            string[] names = { "Помощь", "Вызов", "Чтение", "Правка", "Копия", "НовИмя", "НовКат", "Удал-е", "Меню", "Выход" };

            int x = 0;
            for (int i = 0; i < names.Length; i++)
            {
                if (x >= W) break;

                string num = (i + 1).ToString();
                Put(x, y, num, ConsoleColor.White, ConsoleColor.Black);
                x += num.Length;
                Put(x, y, names[i], ConsoleColor.Black, ConsoleColor.Cyan);
                x += names[i].Length;

                if (i != names.Length - 1)
                {
                    Put(x, y, " ", ConsoleColor.White, ConsoleColor.Black);
                    x++;
                }
            }
        }

        static void PaintBox(int x, int y, int w, int h)
        {
            if (w < 2 || h < 2) return;

            Put(x, y, UL.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            for (int i = 1; i < w - 1; i++)
                Put(x + i, y, HLine.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            Put(x + w - 1, y, UR.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);

            for (int row = 1; row < h - 1; row++)
            {
                Put(x, y + row, VLine.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
                Put(x + w - 1, y + row, VLine.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            }

            Put(x, y + h - 1, DL.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            for (int i = 1; i < w - 1; i++)
                Put(x + i, y + h - 1, HLine.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            Put(x + w - 1, y + h - 1, DR.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
        }

        static void PaintSplit(int x, int y, int w)
        {
            if (w < 2) return;

            Put(x, y, LT.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            for (int i = 1; i < w - 1; i++)
                Put(x + i, y, HLine.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            Put(x + w - 1, y, RT.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
        }

        static void PaintCaption(int boxX, int boxY, int boxW, string title, bool active)
        {
            string t = " " + title + " ";
            if (t.Length > boxW) t = t.Substring(0, Math.Max(0, boxW));

            int xx = boxX + Math.Max(0, (boxW - t.Length) / 2);
            if (active)
                Put(xx, boxY, t, ConsoleColor.Black, ConsoleColor.Cyan);
            else
                Put(xx, boxY, t, ConsoleColor.White, ConsoleColor.DarkBlue);
        }

        static void FillArea(int x, int y, int w, int h, ConsoleColor fg, ConsoleColor bg)
        {
            if (w <= 0 || h <= 0) return;

            string line = new string(' ', w);
            for (int r = 0; r < h; r++)
                Put(x, y + r, line, fg, bg);
        }

        static void Put(int x, int y, string text, ConsoleColor fg, ConsoleColor bg)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (y < 0 || y >= H || x < 0 || x >= W)
                return;
            if (x + text.Length > W)
                text = text.Substring(0, W - x);
            if (text.Length == 0)
                return;

            try
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = fg;
                Console.BackgroundColor = bg;
                Console.Write(text);
            }
            catch
            {
               
            }
        }

        static string FitLeft(string s, int len)
        {
            if (len <= 0) return "";
            if (s.Length > len)
                return s.Substring(0, len);
            return s.PadRight(len);
        }

        static string FitRight(string s, int len)
        {
            if (len <= 0) return "";
            if (s.Length > len)
                return s.Substring(0, len);
            return s.PadLeft(len);
        }
    }
}
