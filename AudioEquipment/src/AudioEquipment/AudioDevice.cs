using System;
using System.Globalization;
using System.Text;

namespace AudioEquipment;

/// <summary>
/// Базовый класс "Аудио- и видеотехника".
/// Хранит характеристики, общие для всех видов техники.
/// </summary>
public class AudioDevice
{
    // ---- значения по умолчанию (вместо "магических констант" в коде) ----
    protected const string DefaultManufacturer = "Не указан";
    protected const string DefaultModel = "Не указана";
    protected const string DefaultColor = "Black";
    protected const int DefaultPowerWatts = 50;
    protected const decimal DefaultPrice = 1000m;

    // ---- границы допустимых значений ----
    protected const int MinPowerWatts = 1;
    protected const decimal MinPrice = 0.01m;

    /// <summary>Формат вывода денежной суммы.</summary>
    protected const string PriceFormat = "0.00";

    private string manufacturer = DefaultManufacturer; // производитель
    private string model = DefaultModel;               // модель
    private string color = DefaultColor;               // цвет корпуса
    private int powerWatts = DefaultPowerWatts;        // потребляемая мощность, Вт
    private decimal price = DefaultPrice;              // цена, руб.

    /// <summary>Конструктор по умолчанию: характеристики получают
    /// стандартные значения.</summary>
    public AudioDevice()
    {
    }

    /// <summary>Конструктор с параметрами. Значения проверяются
    /// свойствами; при недопустимом значении выбрасывается исключение.</summary>
    public AudioDevice(string manufacturer, string model, string color,
                       int powerWatts, decimal price)
    {
        Manufacturer = manufacturer;
        Model = model;
        Color = color;
        PowerWatts = powerWatts;
        Price = price;
    }

    /// <summary>Фирма-производитель. Пустая строка недопустима.</summary>
    public string Manufacturer
    {
        get { return manufacturer; }
        set
        {
            ValidateNotEmpty(value, nameof(Manufacturer));
            manufacturer = value;
        }
    }

    /// <summary>Название модели. Пустая строка недопустима.</summary>
    public string Model
    {
        get { return model; }
        set
        {
            ValidateNotEmpty(value, nameof(Model));
            model = value;
        }
    }

    /// <summary>Цвет корпуса. Пустая строка недопустима.</summary>
    public string Color
    {
        get { return color; }
        set
        {
            ValidateNotEmpty(value, nameof(Color));
            color = value;
        }
    }

    /// <summary>Потребляемая мощность в ваттах (не меньше MinPowerWatts).</summary>
    public int PowerWatts
    {
        get { return powerWatts; }
        set
        {
            ValidateAtLeast(value, MinPowerWatts, nameof(PowerWatts));
            powerWatts = value;
        }
    }

    /// <summary>Цена в рублях (не меньше MinPrice).</summary>
    public decimal Price
    {
        get { return price; }
        set
        {
            ValidateAtLeast(value, MinPrice, nameof(Price));
            price = value;
        }
    }

    /// <summary>Название типа устройства. Виртуальное свойство:
    /// имя GetType() занято классом System.Object.</summary>
    public virtual string DeviceType
    {
        get { return "Аудио/видеотехника"; }
    }

    /// <summary>Текстовое описание объекта. Виртуальный метод
    /// System.Object.ToString(); вызывается методом Console.WriteLine().</summary>
    public override string ToString()
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine("Тип: " + DeviceType);
        text.AppendLine("Производитель: " + Manufacturer);
        text.AppendLine("Модель: " + Model);
        text.AppendLine("Цвет: " + Color);
        text.AppendLine("Мощность: " + PowerWatts + " Вт");
        text.Append("Цена: " + FormatPrice(Price) + " руб.");
        return text.ToString();
    }

    /// <summary>Проверка строкового значения на пустоту.</summary>
    protected static void ValidateNotEmpty(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Значение свойства " + propertyName + " не может быть пустым",
                propertyName);
        }
    }

    /// <summary>Проверка целого значения на нижнюю границу.</summary>
    protected static void ValidateAtLeast(int value, int minValue,
                                          string propertyName)
    {
        if (value < minValue)
        {
            throw new ArgumentOutOfRangeException(propertyName, value,
                "Значение свойства " + propertyName + " должно быть не меньше "
                + minValue);
        }
    }

    /// <summary>Проверка вещественного значения на нижнюю границу.</summary>
    protected static void ValidateAtLeast(double value, double minValue,
                                          string propertyName)
    {
        if (value < minValue)
        {
            throw new ArgumentOutOfRangeException(propertyName, value,
                "Значение свойства " + propertyName + " должно быть не меньше "
                + minValue.ToString(CultureInfo.InvariantCulture));
        }
    }

    /// <summary>Проверка денежного значения на нижнюю границу.</summary>
    protected static void ValidateAtLeast(decimal value, decimal minValue,
                                          string propertyName)
    {
        if (value < minValue)
        {
            throw new ArgumentOutOfRangeException(propertyName, value,
                "Значение свойства " + propertyName + " должно быть не меньше "
                + minValue.ToString(CultureInfo.InvariantCulture));
        }
    }

    /// <summary>Преобразование логического значения в строку "да"/"нет".</summary>
    protected static string BoolToString(bool value)
    {
        return value ? "да" : "нет";
    }

    /// <summary>Форматирование денежной суммы.</summary>
    protected static string FormatPrice(decimal value)
    {
        return value.ToString(PriceFormat, CultureInfo.InvariantCulture);
    }

    /// <summary>Форматирование вещественного числа.</summary>
    protected static string FormatNumber(double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
