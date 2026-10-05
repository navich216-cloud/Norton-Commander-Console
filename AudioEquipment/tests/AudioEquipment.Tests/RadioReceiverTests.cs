using System;
using Xunit;

namespace AudioEquipment.Tests;

/// <summary>Модульные тесты производного класса RadioReceiver.</summary>
public class RadioReceiverTests
{
    [Fact(DisplayName = "Радиоприёмник является наследником AudioDevice")]
    public void RadioReceiver_IsAudioDevice()
    {
        RadioReceiver radio = new RadioReceiver();

        Assert.IsAssignableFrom<AudioDevice>(radio);
    }

    [Fact(DisplayName = "Конструктор по умолчанию задаёт параметры приёмника")]
    public void DefaultConstructor_SetsRadioDefaults()
    {
        RadioReceiver radio = new RadioReceiver();

        Assert.Equal("Sony", radio.Manufacturer);
        Assert.Equal("ICF-306", radio.Model);
        Assert.Equal(5, radio.PowerWatts);
        Assert.Equal(3490m, radio.Price);
        Assert.Equal(87.5, radio.FrequencyMHzMin);
        Assert.Equal(108.0, radio.FrequencyMHzMax);
        Assert.Equal(20, radio.NumberOfPresets);
        Assert.False(radio.HasBluetooth);
    }

    [Fact(DisplayName = "Конструктор с параметрами заполняет свои и общие свойства")]
    public void ParameterizedConstructor_SetsOwnAndInheritedProperties()
    {
        RadioReceiver radio = new RadioReceiver("Philips", "AE5020/12", "Silver",
                                                8, 5490m, 87.5, 108.0, 40, true);

        Assert.Equal("Philips", radio.Manufacturer);
        Assert.Equal(8, radio.PowerWatts);
        Assert.Equal(40, radio.NumberOfPresets);
        Assert.True(radio.HasBluetooth);
    }

    [Fact(DisplayName = "Верхняя граница не больше нижней вызывает исключение")]
    public void FrequencyMax_NotGreaterThanMin_Throws()
    {
        RadioReceiver radio = new RadioReceiver();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => radio.FrequencyMHzMax = 50.0);
    }

    [Fact(DisplayName = "Недопустимая нижняя граница вызывает исключение")]
    public void FrequencyMin_BelowMinimum_Throws()
    {
        RadioReceiver radio = new RadioReceiver();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => radio.FrequencyMHzMin = 0.0);
    }

    [Theory(DisplayName = "Недопустимое количество пресетов вызывает исключение")]
    [InlineData(0)]
    [InlineData(-10)]
    public void NumberOfPresets_BelowMinimum_Throws(int value)
    {
        RadioReceiver radio = new RadioReceiver();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => radio.NumberOfPresets = value);
    }

    [Fact(DisplayName = "Корректный диапазон частот сохраняется")]
    public void FrequencyRange_ValidValues_AreStored()
    {
        RadioReceiver radio = new RadioReceiver();

        radio.FrequencyMHzMin = 64.0;
        radio.FrequencyMHzMax = 74.0;

        Assert.Equal(64.0, radio.FrequencyMHzMin);
        Assert.Equal(74.0, radio.FrequencyMHzMax);
    }

    [Fact(DisplayName = "DeviceType радиоприёмника переопределён")]
    public void DeviceType_ReturnsRadioReceiver()
    {
        RadioReceiver radio = new RadioReceiver();

        Assert.Equal("Радиоприёмник", radio.DeviceType);
    }

    [Fact(DisplayName = "ToString приёмника дополняет описание базового класса")]
    public void ToString_ContainsFrequencyRange()
    {
        RadioReceiver radio = new RadioReceiver("Philips", "AE5020/12", "Silver",
                                                8, 5490m, 87.5, 108.0, 40, true);

        string text = radio.ToString();

        Assert.Contains("Тип: Радиоприёмник", text);
        Assert.Contains("Диапазон частот: 87.5 - 108 МГц", text);
        Assert.Contains("Количество пресетов: 40", text);
        Assert.Contains("Bluetooth: да", text);
    }
}
