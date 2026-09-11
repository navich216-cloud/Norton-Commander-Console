using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp10
{
    // Один элемент списка (файл или папка)
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

    // Имитация Norton Commander (вариант 2)
    class Program
    {
        // размеры окна
        const int W = 80;
        const int H = 25;
        const int Half = 40;
        const int TopY = 1;
        const int BoxH = 22;
        const int Rows = 17;

        // рамка
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

            // слева — по расширению, справа — по имени
            List<NcFile> leftList = new List<NcFile>(all);
            List<NcFile> rightList = new List<NcFile>(all);

            leftList.Sort(SortByExt);
            rightList.Sort(SortByName);

            bool running = true;
            while (running)
            {
                PaintMenu();
                PaintLeft(leftList);
                PaintRight(rightList);
                PaintPrompt();
                PaintFKeys();

                Console.SetCursorPosition(6, 23);
                Console.CursorVisible = true;

                // Ждем нажатия клавиши
                ConsoleKeyInfo key = Console.ReadKey(true);
                Console.CursorVisible = false;

                // Выход по Escape или F10
                if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.F10)
                {
                    running = false;
                }
            }

            // Корректный выход
            Console.Clear();
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void PrepareWindow()
        {
            Console.Title = "Norton Commander";
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            try
            {
                // Сначала уменьшаем окно до минимального размера
                Console.SetWindowSize(80, 25);

                // Затем устанавливаем размер буфера
                Console.SetBufferSize(80, 25);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Если окно слишком большое, сначала делаем буфер больше окна
                try
                {
                    int maxWidth = Math.Max(80, Console.WindowWidth);
                    int maxHeight = Math.Max(25, Console.WindowHeight);

                    Console.SetBufferSize(maxWidth, maxHeight);
                    Console.SetWindowSize(80, 25);
                }
                catch
                {
                    // Если не получается, работаем с текущим размером
                    Console.Clear();
                    Console.WriteLine("Не удалось установить размер консоли 80x25.");
                    Console.WriteLine("Программа будет работать с текущим размером окна.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey(true);
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Ошибка инициализации консоли: {ex.Message}");
                Console.WriteLine("Попробуйте:");
                Console.WriteLine("1. Уменьшить окно консоли (не разворачивать на весь экран)");
                Console.WriteLine("2. Изменить шрифт на меньший размер");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey(true);
                Environment.Exit(1);
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
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

            Put(75, 0, " 8 30", ConsoleColor.Black, ConsoleColor.Cyan);
        }

        static void PaintLeft(List<NcFile> files)
        {
            int x0 = 0;
            FillArea(x0, TopY, Half, BoxH, ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            PaintBox(x0, TopY, Half, BoxH);
            PaintCaption(x0, TopY, Half, @"C:\NC", false);
            PaintSplit(x0, TopY + 19, Half);

            string head = FitLeft("C:\u2193 Имя", 12) + Col + FitLeft("Имя", 12) + Col + FitLeft("Имя", 12);
            Put(x0 + 1, TopY + 1, head, ConsoleColor.Yellow, ConsoleColor.DarkBlue);

            int maxShow = Rows * 3;
            int count = Math.Min(files.Count, maxShow);

            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    int yy = TopY + 2 + r;
                    if (c < 2)
                        Put(x0 + 13 + c * 13, yy, Col.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);

                    int idx = c * Rows + r;
                    if (idx >= count)
                        continue;

                    NcFile f = files[idx];
                    Put(x0 + 1 + c * 13, yy, MakeDosName(f.FileName), ColorOf(f), ConsoleColor.DarkBlue);
                }
            }

            if (files.Count > 0)
                PaintStatus(x0 + 1, TopY + 20, files[0]);
        }

        static void PaintRight(List<NcFile> files)
        {
            int x0 = Half;
            FillArea(x0, TopY, Half, BoxH, ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            PaintBox(x0, TopY, Half, BoxH);
            PaintCaption(x0, TopY, Half, @"C:\NC", true);
            PaintSplit(x0, TopY + 19, Half);

            Put(x0 + 1, TopY + 1, FitLeft("C:\u2193 Имя", 12), ConsoleColor.Yellow, ConsoleColor.DarkBlue);
            Put(x0 + 13, TopY + 1, Col.ToString(), ConsoleColor.Yellow, ConsoleColor.DarkBlue);
            Put(x0 + 14, TopY + 1, FitRight("Размер", 9), ConsoleColor.Yellow, ConsoleColor.DarkBlue);
            Put(x0 + 23, TopY + 1, Col.ToString(), ConsoleColor.Yellow, ConsoleColor.DarkBlue);
            Put(x0 + 24, TopY + 1, FitLeft("  Дата", 8), ConsoleColor.Yellow, ConsoleColor.DarkBlue);
            Put(x0 + 32, TopY + 1, Col.ToString(), ConsoleColor.Yellow, ConsoleColor.DarkBlue);
            Put(x0 + 33, TopY + 1, FitLeft(" Время", 6), ConsoleColor.Yellow, ConsoleColor.DarkBlue);

            int count = Math.Min(files.Count, Rows);

            for (int i = 0; i < count; i++)
                PaintRow(x0 + 1, TopY + 2 + i, files[i], i == 0);

            if (files.Count > 0)
                PaintStatus(x0 + 1, TopY + 20, files[0]);
        }

        static void PaintRow(int x, int y, NcFile f, bool selected)
        {
            ConsoleColor back = ConsoleColor.DarkBlue;
            ConsoleColor front = ColorOf(f);
            ConsoleColor line = ConsoleColor.Cyan;

            if (selected)
            {
                back = ConsoleColor.Cyan;
                front = ConsoleColor.Black;
                line = ConsoleColor.Black;
            }

            string sizeStr = f.IsFolder ? "\u25BAКАТАЛОГ\u25C4" : f.FileSize.ToString();
            ConsoleColor sizeCol = selected ? ConsoleColor.Black : (f.IsFolder ? ConsoleColor.Cyan : front);

            Put(x, y, MakeDosName(f.FileName), front, back);
            Put(x + 12, y, Col.ToString(), line, back);
            Put(x + 13, y, FitRight(sizeStr, 9), sizeCol, back);
            Put(x + 22, y, Col.ToString(), line, back);
            Put(x + 23, y, FitLeft(f.ModDate, 8), selected ? ConsoleColor.Black : ConsoleColor.Cyan, back);
            Put(x + 31, y, Col.ToString(), line, back);
            Put(x + 32, y, FitRight(f.ModTime, 6), selected ? ConsoleColor.Black : ConsoleColor.Cyan, back);
        }

        static void PaintStatus(int x, int y, NcFile f)
        {
            Put(x, y, new string(' ', 38), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            Put(x, y, FitLeft(f.FileName, 12), ConsoleColor.Yellow, ConsoleColor.DarkBlue);

            string mid = f.IsFolder ? "\u25BAКАТАЛОГ\u25C4" : f.FileSize.ToString();

            Put(x + 12, y, FitRight(mid, 9), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            Put(x + 22, y, " " + FitLeft(f.ModDate, 8), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            Put(x + 31, y, " " + FitRight(f.ModTime, 5), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
        }

        static void PaintPrompt()
        {
            Put(0, 23, new string(' ', W), ConsoleColor.Gray, ConsoleColor.Black);
            Put(0, 23, @"C:\NC>", ConsoleColor.Gray, ConsoleColor.Black);
        }

        static void PaintFKeys()
        {
            Put(0, 24, new string(' ', W), ConsoleColor.White, ConsoleColor.Black);

            string[] names = { "Помощь", "Вызов", "Чтение", "Правка", "Копия", "НовИмя", "НовКат", "Удал-е", "Меню", "Выход" };

            int x = 0;
            for (int i = 0; i < names.Length; i++)
            {
                string num = (i + 1).ToString();
                Put(x, 24, num, ConsoleColor.White, ConsoleColor.Black);
                x += num.Length;
                Put(x, 24, names[i], ConsoleColor.Black, ConsoleColor.Cyan);
                x += names[i].Length;

                if (i != names.Length - 1)
                {
                    Put(x, 24, " ", ConsoleColor.White, ConsoleColor.Black);
                    x++;
                }
            }
        }

        static void PaintBox(int x, int y, int w, int h)
        {
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
            Put(x, y, LT.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            for (int i = 1; i < w - 1; i++)
                Put(x + i, y, HLine.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            Put(x + w - 1, y, RT.ToString(), ConsoleColor.Cyan, ConsoleColor.DarkBlue);
        }

        static void PaintCaption(int boxX, int boxY, int boxW, string title, bool active)
        {
            string t = " " + title + " ";
            int xx = boxX + (boxW - t.Length) / 2;
            if (active)
                Put(xx, boxY, t, ConsoleColor.Black, ConsoleColor.Cyan);
            else
                Put(xx, boxY, t, ConsoleColor.White, ConsoleColor.DarkBlue);
        }

        static void FillArea(int x, int y, int w, int h, ConsoleColor fg, ConsoleColor bg)
        {
            string line = new string(' ', w);
            for (int r = 0; r < h; r++)
                Put(x, y + r, line, fg, bg);
        }

        static void Put(int x, int y, string text, ConsoleColor fg, ConsoleColor bg)
        {
            if (y < 0 || y >= H || x < 0)
                return;
            if (x + text.Length > W)
                text = text.Substring(0, W - x);
            if (text.Length == 0)
                return;

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(text);
        }

        static string FitLeft(string s, int len)
        {
            if (s.Length > len)
                return s.Substring(0, len);
            return s.PadRight(len);
        }

        static string FitRight(string s, int len)
        {
            if (s.Length > len)
                return s.Substring(0, len);
            return s.PadLeft(len);
        }
    }
}