using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AudioEquipment;

/// <summary>Пункты главного меню программы.</summary>
public enum MenuCommand
{
    Exit = 0,
    AddDevice = 1,
    ShowAll = 2,
    Demonstration = 3
}

/// <summary>Типы объектов, которые может создать пользователь.</summary>
public enum DeviceKind
{
    AudioDevice = 1,
    Television = 2,
    RadioReceiver = 3
}

/// <summary>Тестирующая программа: демонстрация работы классов,
/// коллекции ссылок на базовый класс и полиморфизма.</summary>
public static class Program
{
    private const int MinMenuCommand = (int)MenuCommand.Exit;
    private const int MinDeviceKind = (int)DeviceKind.AudioDevice;
    private const int MinPowerWatts = 1;
    private const int MinScreenDiagonalInch = 1;
    private const int MinNumberOfPresets = 1;
    private const decimal MinPrice = 0.01m;
    private const double MinFrequencyMHz = 0.1;

    public static void Main()
    {
        ConfigureConsole();
        DemonstratePolymorphism();
        RunMenu();
    }

    /// <summary>Настройка кодировки и формата чисел консоли.</summary>
    private static void ConfigureConsole()
    {
        Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    /// <summary>Демонстрация полиморфизма: объекты трёх классов
    /// хранятся в одной коллекции ссылок на базовый класс.</summary>
    private static void DemonstratePolymorphism()
    {
        List<AudioDevice> devices = new List<AudioDevice>
        {
            new AudioDevice("Philips", "TAR8805/10", "Silver", 10, 7990m),
            new Television("LG", "55NANO856PA", "Black", 95, 59990m,
                           55, "NanoCell", "3840x2160", true),
            new RadioReceiver("Philips", "AE5020/12", "Silver", 8, 5490m,
                              87.5, 108.0, 40, true)
        };

        Console.WriteLine("=== Демонстрация работы классов ===");
        foreach (AudioDevice device in devices)
        {
            // вызывается метод ToString() реального типа объекта
            Console.WriteLine();
            Console.WriteLine(device);
        }

        Console.WriteLine();
        Console.WriteLine("=== Изменение характеристик через свойства ===");
        Television tv = (Television)devices[1];
        tv.Price = 54990m;
        tv.Resolution = "7680x4320";
        RadioReceiver radio = (RadioReceiver)devices[2];
        radio.Color = "Black";
        radio.HasBluetooth = false;
        Console.WriteLine();
        Console.WriteLine(tv);
        Console.WriteLine();
        Console.WriteLine(radio);

        Console.WriteLine();
        Console.WriteLine("=== Раннее и позднее связывание ===");
        AudioDevice baseVariable = devices[0]; // объект базового класса
        ref AudioDevice reference = ref baseVariable; // ссылка (ref-переменная)
        AudioDevice derivedInBase = tv; // переменная базового типа

        Console.WriteLine("Ссылка на объект базового класса -> "
                          + reference.DeviceType);
        Console.WriteLine("Переменная базового типа на объект телевизора -> "
                          + derivedInBase.DeviceType);
        Console.WriteLine("Проверка обработки ошибок:");
        try
        {
            tv.PowerWatts = 0; // недопустимое значение
        }
        catch (ArgumentException error)
        {
            Console.WriteLine("Перехвачено исключение: " + error.Message);
        }
    }

    /// <summary>Работа со списком объектов в диалоговом режиме.</summary>
    private static void RunMenu()
    {
        List<AudioDevice> devices = new List<AudioDevice>();
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Добавить объект");
            Console.WriteLine("2. Вывести список");
            Console.WriteLine("3. Повторить демонстрацию полиморфизма");
            Console.WriteLine("0. Завершить работу");
            MenuCommand command =
                (MenuCommand)ReadInt("Ваш выбор: ", MinMenuCommand);

            switch (command)
            {
                case MenuCommand.AddDevice:
                    AddDevice(devices);
                    break;
                case MenuCommand.ShowAll:
                    PrintList(devices);
                    break;
                case MenuCommand.Demonstration:
                    DemonstratePolymorphism();
                    break;
                case MenuCommand.Exit:
                    devices.Clear();
                    Console.WriteLine("Программа завершена.");
                    return;
                default:
                    Console.WriteLine("Неизвестный пункт меню.");
                    break;
            }
        }
    }

    /// <summary>Вывод содержимого коллекции через виртуальный метод.</summary>
    private static void PrintList(List<AudioDevice> devices)
    {
        if (devices.Count == 0)
        {
            Console.WriteLine("Список пуст.");
            return;
        }
        for (int i = 0; i < devices.Count; i++)
        {
            Console.WriteLine();
            Console.WriteLine("Объект #" + (i + 1));
            Console.WriteLine(devices[i]);
        }
    }

    /// <summary>Создание объекта выбранного типа и добавление его в список.</summary>
    private static void AddDevice(List<AudioDevice> devices)
    {
        Console.WriteLine();
        Console.WriteLine("Выберите тип объекта:");
        Console.WriteLine("1. Аудио/видеотехника (базовый класс)");
        Console.WriteLine("2. Телевизор");
        Console.WriteLine("3. Радиоприёмник");
        DeviceKind kind = (DeviceKind)ReadInt("Ваш выбор: ", MinDeviceKind);

        try
        {
            switch (kind)
            {
                case DeviceKind.AudioDevice:
                    devices.Add(CreateAudioDevice());
                    break;
                case DeviceKind.Television:
                    devices.Add(CreateTelevision());
                    break;
                case DeviceKind.RadioReceiver:
                    devices.Add(CreateRadioReceiver());
                    break;
                default:
                    Console.WriteLine("Неизвестный тип объекта.");
                    return;
            }
            Console.WriteLine("Объект добавлен.");
        }
        catch (ArgumentException error)
        {
            Console.WriteLine("Объект не создан: " + error.Message);
        }
    }

    private static AudioDevice CreateAudioDevice()
    {
        return new AudioDevice(ReadLine("Производитель: "),
                               ReadLine("Модель: "),
                               ReadLine("Цвет: "),
                               ReadInt("Мощность, Вт: ", MinPowerWatts),
                               ReadDecimal("Цена, руб.: ", MinPrice));
    }

    private static AudioDevice CreateTelevision()
    {
        return new Television(ReadLine("Производитель: "),
                              ReadLine("Модель: "),
                              ReadLine("Цвет: "),
                              ReadInt("Мощность, Вт: ", MinPowerWatts),
                              ReadDecimal("Цена, руб.: ", MinPrice),
                              ReadInt("Диагональ экрана, дюйм: ",
                                      MinScreenDiagonalInch),
                              ReadLine("Тип экрана (LED/OLED/QLED): "),
                              ReadLine("Разрешение (напр. 1920x1080): "),
                              ReadBool("Smart TV"));
    }

    private static AudioDevice CreateRadioReceiver()
    {
        return new RadioReceiver(ReadLine("Производитель: "),
                                 ReadLine("Модель: "),
                                 ReadLine("Цвет: "),
                                 ReadInt("Мощность, Вт: ", MinPowerWatts),
                                 ReadDecimal("Цена, руб.: ", MinPrice),
                                 ReadDouble("Нижняя граница, МГц: ",
                                            MinFrequencyMHz),
                                 ReadDouble("Верхняя граница, МГц: ",
                                            MinFrequencyMHz),
                                 ReadInt("Количество пресетов: ",
                                         MinNumberOfPresets),
                                 ReadBool("Bluetooth"));
    }

    /// <summary>Ввод целого числа, не меньшего minValue.</summary>
    private static int ReadInt(string prompt, int minValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string line = Console.ReadLine();
            if (line == null)
            {
                return minValue; // конец потока ввода
            }
            int value;
            if (int.TryParse(line.Trim(), NumberStyles.Integer,
                             CultureInfo.CurrentCulture, out value)
                && value >= minValue)
            {
                return value;
            }
            Console.WriteLine("Ошибка ввода. Введите целое число >= "
                              + minValue + ".");
        }
    }

    /// <summary>Ввод вещественного числа, не меньшего minValue.</summary>
    private static double ReadDouble(string prompt, double minValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string line = Console.ReadLine();
            if (line == null)
            {
                return minValue;
            }
            double value;
            if (double.TryParse(line.Trim(), NumberStyles.Float,
                                CultureInfo.CurrentCulture, out value)
                && value >= minValue)
            {
                return value;
            }
            Console.WriteLine("Ошибка ввода. Введите число >= "
                              + minValue.ToString(CultureInfo.CurrentCulture)
                              + ".");
        }
    }

    /// <summary>Ввод денежной суммы, не меньшей minValue.</summary>
    private static decimal ReadDecimal(string prompt, decimal minValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string line = Console.ReadLine();
            if (line == null)
            {
                return minValue;
            }
            decimal value;
            if (decimal.TryParse(line.Trim(), NumberStyles.Number,
                                 CultureInfo.CurrentCulture, out value)
                && value >= minValue)
            {
                return value;
            }
            Console.WriteLine("Ошибка ввода. Введите число >= "
                              + minValue.ToString(CultureInfo.CurrentCulture)
                              + ".");
        }
    }

    /// <summary>Ввод непустой строки.</summary>
    private static string ReadLine(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine();
            if (value == null)
            {
                return "Не указано";
            }
            if (value.Trim().Length > 0)
            {
                return value.Trim();
            }
            Console.WriteLine("Ошибка ввода. Строка не может быть пустой.");
        }
    }

    /// <summary>Ввод логического значения.</summary>
    private static bool ReadBool(string prompt)
    {
        const int No = 0;
        const int Yes = 1;
        while (true)
        {
            int value = ReadInt(prompt + " (1 - да, 0 - нет): ", No);
            if (value == No || value == Yes)
            {
                return value == Yes;
            }
            Console.WriteLine("Ошибка ввода. Введите 1 или 0.");
        }
    }
}
