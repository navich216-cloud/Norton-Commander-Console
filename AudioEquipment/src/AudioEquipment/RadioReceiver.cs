using System;
using System.Text;

namespace AudioEquipment;

/// <summary>
/// Производный класс "Радиоприёмник".
/// Добавляет к общим характеристикам параметры приёма.
/// </summary>
public class RadioReceiver : AudioDevice
{
    // ---- значения по умолчанию ----
    private const string DefaultRadioManufacturer = "Sony";
    private const string DefaultRadioModel = "ICF-306";
    private const string DefaultRadioColor = "Black";
    private const int DefaultRadioPowerWatts = 5;
    private const decimal DefaultRadioPrice = 3490m;
    private const double DefaultFrequencyMHzMin = 87.5;
    private const double DefaultFrequencyMHzMax = 108.0;
    private const int DefaultNumberOfPresets = 20;
    private const bool DefaultHasBluetooth = false;

    // ---- границы допустимых значений ----
    private const double MinFrequencyMHz = 0.1;
    private const int MinNumberOfPresets = 1;

    private double frequencyMHzMin = DefaultFrequencyMHzMin; // нижняя граница
    private double frequencyMHzMax = DefaultFrequencyMHzMax; // верхняя граница
    private int numberOfPresets = DefaultNumberOfPresets;    // число пресетов
    private bool hasBluetooth = DefaultHasBluetooth;         // наличие Bluetooth

    /// <summary>Конструктор по умолчанию: вызывает конструктор базового
    /// класса со стандартными значениями радиоприёмника.</summary>
    public RadioReceiver()
        : base(DefaultRadioManufacturer, DefaultRadioModel, DefaultRadioColor,
               DefaultRadioPowerWatts, DefaultRadioPrice)
    {
    }

    /// <summary>Конструктор с параметрами: общие характеристики передаются
    /// конструктору базового класса.</summary>
    public RadioReceiver(string manufacturer, string model, string color,
                         int powerWatts, decimal price,
                         double frequencyMHzMin, double frequencyMHzMax,
                         int numberOfPresets, bool hasBluetooth)
        : base(manufacturer, model, color, powerWatts, price)
    {
        FrequencyMHzMin = frequencyMHzMin;
        FrequencyMHzMax = frequencyMHzMax;
        NumberOfPresets = numberOfPresets;
        HasBluetooth = hasBluetooth;
    }

    /// <summary>Нижняя граница диапазона частот, МГц.</summary>
    public double FrequencyMHzMin
    {
        get { return frequencyMHzMin; }
        set
        {
            ValidateAtLeast(value, MinFrequencyMHz, nameof(FrequencyMHzMin));
            frequencyMHzMin = value;
        }
    }

    /// <summary>Верхняя граница диапазона частот, МГц.
    /// Должна быть больше нижней границы.</summary>
    public double FrequencyMHzMax
    {
        get { return frequencyMHzMax; }
        set
        {
            if (value <= frequencyMHzMin)
            {
                throw new ArgumentOutOfRangeException(nameof(FrequencyMHzMax),
                    value, "Верхняя граница диапазона должна быть больше нижней ("
                    + FormatNumber(frequencyMHzMin) + " МГц)");
            }
            frequencyMHzMax = value;
        }
    }

    /// <summary>Количество ячеек памяти (пресетов).</summary>
    public int NumberOfPresets
    {
        get { return numberOfPresets; }
        set
        {
            ValidateAtLeast(value, MinNumberOfPresets, nameof(NumberOfPresets));
            numberOfPresets = value;
        }
    }

    /// <summary>Наличие модуля Bluetooth.</summary>
    public bool HasBluetooth
    {
        get { return hasBluetooth; }
        set { hasBluetooth = value; }
    }

    /// <summary>Переопределённое название типа устройства.</summary>
    public override string DeviceType
    {
        get { return "Радиоприёмник"; }
    }

    /// <summary>Переопределённое текстовое описание: к сведениям базового
    /// класса добавляются характеристики приёма.</summary>
    public override string ToString()
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine(base.ToString());
        text.AppendLine("Диапазон частот: " + FormatNumber(FrequencyMHzMin)
                        + " - " + FormatNumber(FrequencyMHzMax) + " МГц");
        text.AppendLine("Количество пресетов: " + NumberOfPresets);
        text.Append("Bluetooth: " + BoolToString(HasBluetooth));
        return text.ToString();
    }
}
