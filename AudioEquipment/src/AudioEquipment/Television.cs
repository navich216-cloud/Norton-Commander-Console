using System;
using System.Text;

namespace AudioEquipment;

/// <summary>
/// Производный класс "Телевизор".
/// Добавляет к общим характеристикам параметры экрана.
/// </summary>
public class Television : AudioDevice
{
    // ---- значения по умолчанию ----
    private const string DefaultTvManufacturer = "Samsung";
    private const string DefaultTvModel = "UE43T5300";
    private const string DefaultTvColor = "Black";
    private const int DefaultTvPowerWatts = 65;
    private const decimal DefaultTvPrice = 32990m;
    private const int DefaultScreenDiagonalInch = 43;
    private const string DefaultScreenType = "LED";
    private const string DefaultResolution = "1920x1080";
    private const bool DefaultHasSmartTV = true;

    // ---- границы допустимых значений ----
    private const int MinScreenDiagonalInch = 1;

    private int screenDiagonalInch = DefaultScreenDiagonalInch; // диагональ, дюйм
    private string screenType = DefaultScreenType;              // тип экрана
    private string resolution = DefaultResolution;              // разрешение
    private bool hasSmartTV = DefaultHasSmartTV;                // наличие Smart TV

    /// <summary>Конструктор по умолчанию: вызывает конструктор базового
    /// класса со стандартными значениями телевизора.</summary>
    public Television()
        : base(DefaultTvManufacturer, DefaultTvModel, DefaultTvColor,
               DefaultTvPowerWatts, DefaultTvPrice)
    {
    }

    /// <summary>Конструктор с параметрами: общие характеристики передаются
    /// конструктору базового класса.</summary>
    public Television(string manufacturer, string model, string color,
                      int powerWatts, decimal price, int screenDiagonalInch,
                      string screenType, string resolution, bool hasSmartTV)
        : base(manufacturer, model, color, powerWatts, price)
    {
        ScreenDiagonalInch = screenDiagonalInch;
        ScreenType = screenType;
        Resolution = resolution;
        HasSmartTV = hasSmartTV;
    }

    /// <summary>Диагональ экрана в дюймах (не меньше MinScreenDiagonalInch).</summary>
    public int ScreenDiagonalInch
    {
        get { return screenDiagonalInch; }
        set
        {
            ValidateAtLeast(value, MinScreenDiagonalInch,
                            nameof(ScreenDiagonalInch));
            screenDiagonalInch = value;
        }
    }

    /// <summary>Тип экрана (LED, OLED, QLED и т.д.).</summary>
    public string ScreenType
    {
        get { return screenType; }
        set
        {
            ValidateNotEmpty(value, nameof(ScreenType));
            screenType = value;
        }
    }

    /// <summary>Разрешение экрана, например 1920x1080.</summary>
    public string Resolution
    {
        get { return resolution; }
        set
        {
            ValidateNotEmpty(value, nameof(Resolution));
            resolution = value;
        }
    }

    /// <summary>Наличие функции Smart TV.</summary>
    public bool HasSmartTV
    {
        get { return hasSmartTV; }
        set { hasSmartTV = value; }
    }

    /// <summary>Переопределённое название типа устройства.</summary>
    public override string DeviceType
    {
        get { return "Телевизор"; }
    }

    /// <summary>Переопределённое текстовое описание: к сведениям базового
    /// класса добавляются характеристики экрана.</summary>
    public override string ToString()
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine(base.ToString());
        text.AppendLine("Диагональ экрана: " + ScreenDiagonalInch + " дюйм");
        text.AppendLine("Тип экрана: " + ScreenType);
        text.AppendLine("Разрешение: " + Resolution);
        text.Append("Smart TV: " + BoolToString(HasSmartTV));
        return text.ToString();
    }
}
