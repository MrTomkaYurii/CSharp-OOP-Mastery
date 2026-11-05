using System;
using System.Collections.Generic;

namespace PolymorphismAndCompositionDesign;

/*
 * ── 04. Поліморфізм замість умовних операторів та Композиція проти Спадкування ──
 *
 * Цей приклад демонструє два найважливіші практичні уроки ООП:
 *
 *   Частина 1. Заміна крихких `switch / if-else` на Поліморфізм:
 *     Замість методу з довгим `switch (deliveryType)`, де додавання нового
 *     способу доставки вимагає переписувати старий код, кожен спосіб доставки
 *     інкапсулюється у власному класі, що реалізує `IShippingRule`.
 *
 *   Частина 2. Чому глибоке спадкування (`GameUnit -> FlyingUnit -> ...`)
 *     заходить у глухий кут при комбінації поведінок, і як Композиція
 *     інтерфейсів (`IMovementBehavior`, `IAttackBehavior`) дозволяє
 *     конструювати будь-які комбінації на льоту.
 */

// ── ЧАСТИНА 1: Поліморфний розрахунок доставки (замість switch/if) ────────
public record OrderParcel(string Id, decimal WeightKg, decimal DeclaredValue);

public interface IShippingRule
{
    string CarrierName { get; }
    decimal CalculateCost(OrderParcel parcel);
}

public sealed class CourierShipping : IShippingRule
{
    public string CarrierName => "Міський велокур'єр";
    public decimal CalculateCost(OrderParcel parcel)
        => 80m + (parcel.WeightKg * 15m);
}

public sealed class DroneAirShipping : IShippingRule
{
    public string CarrierName => "Експрес-дрон";
    public decimal CalculateCost(OrderParcel parcel)
        => 150m + (parcel.WeightKg * 40m) + (parcel.DeclaredValue * 0.01m);
}

public sealed class FreightTruckShipping : IShippingRule
{
    public string CarrierName => "Вантажний термінал";
    public decimal CalculateCost(OrderParcel parcel)
        => 200m + (parcel.WeightKg * 5m);
}

// ── ЧАСТИНА 2: Композиція поведінок замість комбінаторного вибуху підкласів ──
public interface IMovementBehavior
{
    string DescribeMove();
}

public interface IAttackBehavior
{
    string DescribeAttack();
}

public sealed class GroundWalkMovement : IMovementBehavior
{
    public string DescribeMove() => "крокує по землі";
}

public sealed class JetPackFlightMovement : IMovementBehavior
{
    public string DescribeMove() => "летить на реактивному ранці";
}

public sealed class MeleeSwordAttack : IAttackBehavior
{
    public string DescribeAttack() => "завдає удару плазмовим мечем";
}

public sealed class LongRangeLaserAttack : IAttackBehavior
{
    public string DescribeAttack() => "стріляє імпульсним лазером здалеку";
}

// Замість 4+ жорстких підкласів (WalkingMeleeUnit, FlyingMeleeUnit, WalkingLaserUnit, FlyingLaserUnit)
// маємо єдиний клас GameUnit, який КОМПОНУЄ будь-який рух і будь-яку атаку, і навіть змінює їх у рантаймі!
public sealed class GameUnit
{
    public string Name { get; }
    private IMovementBehavior _movement;
    private IAttackBehavior _attack;

    public GameUnit(string name, IMovementBehavior movement, IAttackBehavior attack)
    {
        Name = name;
        _movement = movement;
        _attack = attack;
    }

    public void UpgradeMovement(IMovementBehavior newMovement)
        => _movement = newMovement;

    public void PerformTurn()
        => Console.WriteLine($"  [{Name}] {_movement.DescribeMove()} та {_attack.DescribeAttack()}!");
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== 1. Поліморфізм замість розгалужених switch/if ===");
        var parcel = new OrderParcel("ORD-900", WeightKg: 3.5m, DeclaredValue: 5000m);

        // Клієнтський цикл не містить жодного if або switch!
        IShippingRule[] shippingOptions =
        {
            new CourierShipping(),
            new DroneAirShipping(),
            new FreightTruckShipping()
        };

        foreach (IShippingRule rule in shippingOptions)
        {
            Console.WriteLine($"  {rule.CarrierName,-22} -> Вартість: {rule.CalculateCost(parcel):C}");
        }

        Console.WriteLine("\n=== 2. Композиція поведінок замість глибокого спадкування ===");
        var cyberKnight = new GameUnit(
            "Кібер-Лицар",
            new GroundWalkMovement(),
            new MeleeSwordAttack());

        var sniperBot = new GameUnit(
            "Снайпер-Дрон",
            new JetPackFlightMovement(),
            new LongRangeLaserAttack());

        cyberKnight.PerformTurn();
        sniperBot.PerformTurn();

        Console.WriteLine("\n  [Апгрейд у рантаймі]: Кібер-Лицар отримує реактивний ранець без зміни свого класу!");
        cyberKnight.UpgradeMovement(new JetPackFlightMovement());
        cyberKnight.PerformTurn();
    }
}
