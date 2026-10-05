using System;
using Xunit;

namespace AudioEquipment.Tests;

/// <summary>Модульные тесты производного класса Television.</summary>
public class TelevisionTests
{
    [Fact(DisplayName = "Телевизор является наследником AudioDevice")]
    public void Television_IsAudioDevice()
    {
        Television tv = new Television();

        Assert.IsAssignableFrom<AudioDevice>(tv);
    }

    [Fact(DisplayName = "Конструктор по умолчанию задаёт параметры телевизора")]
    public void DefaultConstructor_SetsTelevisionDefaults()
    {
        Television tv = new Television();

        Assert.Equal("Samsung", tv.Manufacturer);
        Assert.Equal("UE43T5300", tv.Model);
        Assert.Equal(65, tv.PowerWatts);
        Assert.Equal(32990m, tv.Price);
        Assert.Equal(43, tv.ScreenDiagonalInch);
        Assert.Equal("LED", tv.ScreenType);
        Assert.Equal("1920x1080", tv.Resolution);
        Assert.True(tv.HasSmartTV);
    }

    [Fact(DisplayName = "Конструктор с параметрами заполняет свои и общие свойства")]
    public void ParameterizedConstructor_SetsOwnAndInheritedProperties()
    {
        Television tv = new Television("LG", "55NANO856PA", "Black", 95, 59990m,
                                       55, "NanoCell", "3840x2160", true);

        Assert.Equal("LG", tv.Manufacturer);
        Assert.Equal(95, tv.PowerWatts);
        Assert.Equal(59990m, tv.Price);
        Assert.Equal(55, tv.ScreenDiagonalInch);
        Assert.Equal("NanoCell", tv.ScreenType);
        Assert.Equal("3840x2160", tv.Resolution);
    }

    [Theory(DisplayName = "Недопустимая диагональ экрана вызывает исключение")]
    [InlineData(0)]
    [InlineData(-32)]
    public void ScreenDiagonalInch_BelowMinimum_Throws(int value)
    {
        Television tv = new Television();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => tv.ScreenDiagonalInch = value);
    }

    [Fact(DisplayName = "Пустой тип экрана вызывает ArgumentException")]
    public void ScreenType_Empty_ThrowsArgumentException()
    {
        Television tv = new Television();

        Assert.Throws<ArgumentException>(() => tv.ScreenType = "  ");
    }

    [Fact(DisplayName = "Свойства телевизора изменяются корректно")]
    public void Properties_CanBeChanged()
    {
        Television tv = new Television();

        tv.Resolution = "7680x4320";
        tv.Price = 54990m;
        tv.HasSmartTV = false;

        Assert.Equal("7680x4320", tv.Resolution);
        Assert.Equal(54990m, tv.Price);
        Assert.False(tv.HasSmartTV);
    }

    [Fact(DisplayName = "DeviceType телевизора переопределён")]
    public void DeviceType_ReturnsTelevision()
    {
        Television tv = new Television();

        Assert.Equal("Телевизор", tv.DeviceType);
    }

    [Fact(DisplayName = "ToString телевизора дополняет описание базового класса")]
    public void ToString_ContainsScreenCharacteristics()
    {
        Television tv = new Television("LG", "55NANO856PA", "Black", 95, 59990m,
                                       55, "NanoCell", "3840x2160", true);

        string text = tv.ToString();

        Assert.Contains("Тип: Телевизор", text);
        Assert.Contains("Производитель: LG", text);
        Assert.Contains("Диагональ экрана: 55 дюйм", text);
        Assert.Contains("Тип экрана: NanoCell", text);
        Assert.Contains("Разрешение: 3840x2160", text);
        Assert.Contains("Smart TV: да", text);
    }
}
