using System;
using Xunit;

namespace AudioEquipment.Tests;

/// <summary>Модульные тесты базового класса AudioDevice.</summary>
public class AudioDeviceTests
{
    [Fact(DisplayName = "Конструктор по умолчанию задаёт стандартные значения")]
    public void DefaultConstructor_SetsDefaultValues()
    {
        AudioDevice device = new AudioDevice();

        Assert.Equal("Не указан", device.Manufacturer);
        Assert.Equal("Не указана", device.Model);
        Assert.Equal("Black", device.Color);
        Assert.Equal(50, device.PowerWatts);
        Assert.Equal(1000m, device.Price);
    }

    [Fact(DisplayName = "Конструктор с параметрами заполняет все свойства")]
    public void ParameterizedConstructor_SetsAllProperties()
    {
        AudioDevice device = new AudioDevice("Philips", "TAR8805/10", "Silver",
                                             10, 7990m);

        Assert.Equal("Philips", device.Manufacturer);
        Assert.Equal("TAR8805/10", device.Model);
        Assert.Equal("Silver", device.Color);
        Assert.Equal(10, device.PowerWatts);
        Assert.Equal(7990m, device.Price);
    }

    [Theory(DisplayName = "Пустой производитель вызывает ArgumentException")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Manufacturer_EmptyValue_ThrowsArgumentException(string value)
    {
        AudioDevice device = new AudioDevice();

        Assert.Throws<ArgumentException>(() => device.Manufacturer = value);
    }

    [Fact(DisplayName = "Пустая модель вызывает ArgumentException")]
    public void Model_EmptyValue_ThrowsArgumentException()
    {
        AudioDevice device = new AudioDevice();

        Assert.Throws<ArgumentException>(() => device.Model = "");
    }

    [Theory(DisplayName = "Недопустимая мощность вызывает исключение")]
    [InlineData(0)]
    [InlineData(-15)]
    public void PowerWatts_BelowMinimum_ThrowsArgumentOutOfRangeException(int value)
    {
        AudioDevice device = new AudioDevice();

        Assert.Throws<ArgumentOutOfRangeException>(() => device.PowerWatts = value);
    }

    [Fact(DisplayName = "Недопустимая цена вызывает исключение")]
    public void Price_BelowMinimum_ThrowsArgumentOutOfRangeException()
    {
        AudioDevice device = new AudioDevice();

        Assert.Throws<ArgumentOutOfRangeException>(() => device.Price = 0m);
    }

    [Fact(DisplayName = "Свойство сохраняет допустимое значение")]
    public void Price_ValidValue_IsStored()
    {
        AudioDevice device = new AudioDevice();

        device.Price = 12345.67m;

        Assert.Equal(12345.67m, device.Price);
    }

    [Fact(DisplayName = "Объект базового класса не изменяется после ошибки")]
    public void InvalidAssignment_KeepsPreviousValue()
    {
        AudioDevice device = new AudioDevice("Sony", "STR-DH190", "Black",
                                             100, 25990m);

        Assert.Throws<ArgumentOutOfRangeException>(() => device.PowerWatts = -1);
        Assert.Equal(100, device.PowerWatts);
    }

    [Fact(DisplayName = "DeviceType базового класса — Аудио/видеотехника")]
    public void DeviceType_ReturnsBaseTypeName()
    {
        AudioDevice device = new AudioDevice();

        Assert.Equal("Аудио/видеотехника", device.DeviceType);
    }

    [Fact(DisplayName = "ToString содержит все общие характеристики")]
    public void ToString_ContainsAllCharacteristics()
    {
        AudioDevice device = new AudioDevice("Philips", "TAR8805/10", "Silver",
                                             10, 7990m);

        string text = device.ToString();

        Assert.Contains("Тип: Аудио/видеотехника", text);
        Assert.Contains("Производитель: Philips", text);
        Assert.Contains("Модель: TAR8805/10", text);
        Assert.Contains("Цвет: Silver", text);
        Assert.Contains("Мощность: 10 Вт", text);
        Assert.Contains("Цена: 7990.00 руб.", text);
    }
}
