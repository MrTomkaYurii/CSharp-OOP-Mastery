# Лекція 4: Приведення Типів в Ієрархіях: Upcasting, Downcasting та Pattern Matching

## 1. Висхідне приведення (Upcasting)

**Upcasting** — це приведення посилання похідного класу до типу його базового класу:
```csharp
Dog dog = new Dog();
Animal animal = dog; // ✅ Upcasting: неявне, 100% безпечне
```
* **Чому безпечно:** Об'єкт `Dog` завжди гарантовано має всі поля та методи `Animal`.
* **Обмеження:** Через змінну `animal` видно лише ті члени, що описані в типі `Animal`. Специфічні методи собаки (`dog.Bark()`) приховані.

---

## 2. Низхідне приведення (Downcasting)

**Downcasting** — це зворотне приведення посилання базового типу до конкретного похідного типу:
```csharp
Animal animal = new Dog();
Dog dog = (Dog)animal; // ⚠️ Явне приведення: потенційно небезпечне!
```
Якщо насправді за посиланням `animal` зберігається `Cat`, виникне фатальний виняток `InvalidCastException`.

---

## 3. Безпечні оператори `is`, `as` та Pattern Matching у сучасному C#

### Оператор `as`:
Повертає типізоване посилання або `null`, якщо тип не збігається (ніколи не кидає виняток):
```csharp
Dog? dog = animal as Dog;
if (dog != null) {
    dog.Bark();
}
```

### Сучасний підхід: Декларативний Pattern Matching (`is` pattern):
Починаючи з C# 7+, найбільш ідіоматичним є одночасна перевірка типу та розпакування в типізовану змінну:
```csharp
if (animal is Dog activeDog)
{
    activeDog.Bark(); // activeDog доступна всередині блоку if
}
```

### Pattern Matching зі switch-виразом:
```csharp
string sound = animal switch
{
    Dog d when d.IsTrained => "Дисципліноване гавкання",
    Dog => "Звичайне гавкання",
    Cat c => "Мяукання",
    _ => "Невідомий звук"
};
```
