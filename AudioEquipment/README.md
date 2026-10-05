# AudioEquipment — лабораторная работа по C#

Консольное приложение .NET 8: иерархия классов «Аудио- и видеотехника»
(вариант №4) и модульные тесты xUnit.

## Состав решения

| Проект | Назначение |
|---|---|
| `src/AudioEquipment` | классы `AudioDevice`, `Television`, `RadioReceiver` и тестовая программа `Program.cs` |
| `tests/AudioEquipment.Tests` | модульные тесты xUnit (37 тестов) |

## Классы

- `AudioDevice` — базовый класс: производитель, модель, цвет, мощность, цена.
  Виртуальный метод `ToString()` и виртуальное свойство `DeviceType`.
- `Television : AudioDevice` — диагональ и тип экрана, разрешение, Smart TV.
- `RadioReceiver : AudioDevice` — диапазон частот, число пресетов, Bluetooth.

Доступ к характеристикам организован через свойства; недопустимые значения
приводят к исключениям `ArgumentException` / `ArgumentOutOfRangeException`.
Полиморфизм демонстрируется на коллекции `List<AudioDevice>`.

## Запуск

```bash
dotnet run --project src/AudioEquipment
```

## Модульные тесты

```bash
dotnet test
```
