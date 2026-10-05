using System;
using System.Collections.Generic;
using Xunit;

namespace AudioEquipment.Tests;

/// <summary>Модульные тесты, проверяющие работу полиморфизма
/// при обращении к объектам через ссылку на базовый класс.</summary>
public class PolymorphismTests
{
    /// <summary>Коллекция ссылок на базовый класс с объектами трёх типов.</summary>
    private static List<AudioDevice> CreateDevices()
    {
        return new List<AudioDevice>
        {
            new AudioDevice("Philips", "TAR8805/10", "Silver", 10, 7990m),
            new Television("LG", "55NANO856PA", "Black", 95, 59990m,
                           55, "NanoCell", "3840x2160", true),
            new RadioReceiver("Philips", "AE5020/12", "Silver", 8, 5490m,
                              87.5, 108.0, 40, true)
        };
    }

    [Fact(DisplayName = "Коллекция хранит объекты трёх разных классов")]
    public void Collection_ContainsObjectsOfAllClasses()
    {
        List<AudioDevice> devices = CreateDevices();

        Assert.Equal(3, devices.Count);
        Assert.IsType<AudioDevice>(devices[0]);
        Assert.IsType<Television>(devices[1]);
        Assert.IsType<RadioReceiver>(devices[2]);
    }

    [Fact(DisplayName = "DeviceType вызывается по фактическому типу объекта")]
    public void DeviceType_ResolvedByRealType()
    {
        List<AudioDevice> devices = CreateDevices();

        Assert.Equal("Аудио/видеотехника", devices[0].DeviceType);
        Assert.Equal("Телевизор", devices[1].DeviceType);
        Assert.Equal("Радиоприёмник", devices[2].DeviceType);
    }

    [Fact(DisplayName = "ToString через ссылку базового типа даёт описание "
                        + "производного класса")]
    public void ToString_ResolvedByRealType()
    {
        List<AudioDevice> devices = CreateDevices();

        Assert.Contains("Диагональ экрана: 55 дюйм", devices[1].ToString());
        Assert.Contains("Количество пресетов: 40", devices[2].ToString());
        Assert.DoesNotContain("Диагональ экрана", devices[0].ToString());
    }

    [Fact(DisplayName = "Описание производного класса включает сведения базового")]
    public void DerivedToString_IncludesBaseInformation()
    {
        AudioDevice device = new Television("LG", "55NANO856PA", "Black", 95,
                                            59990m, 55, "NanoCell",
                                            "3840x2160", true);

        string text = device.ToString();

        Assert.Contains("Производитель: LG", text);   // часть базового класса
        Assert.Contains("Цена: 59990.00 руб.", text); // часть базового класса
        Assert.Contains("Smart TV: да", text);        // часть производного класса
    }

    [Fact(DisplayName = "Ссылка (ref) на переменную базового типа "
                        + "сохраняет полиморфное поведение")]
    public void Reference_KeepsPolymorphicBehaviour()
    {
        AudioDevice variable = new RadioReceiver();
        ref AudioDevice reference = ref variable;

        Assert.Equal("Радиоприёмник", reference.DeviceType);
        Assert.Contains("Диапазон частот", reference.ToString());
    }
}
