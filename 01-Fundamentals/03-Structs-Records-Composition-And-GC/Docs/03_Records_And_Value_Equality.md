# 03. Порівняння об'єктів: `Reference Equality` (`class`) проти `Value Equality` (`record`)

У класичних класах (`class`) оператор `==` та метод `Equals()` за замовчуванням перевіряють **рівність адрес у пам'яті (`ReferenceEquals`)**. Два різні об'єкти з однаковими полями вважаються нерівними.

Позиційний запис **`record`** (наприклад, `public sealed record SubscriptionPlan(string Name, decimal MonthlyPrice, int MaxUsers);`) автоматично генерує:
1. **Структурну рівність (`Value Equality`)**: порівняння за значенням кожного поля ([Program.cs](../03-RecordEqualityAndWith/Program.cs#L44)).
2. **Оператор неруйнівної мутації `with`**: створення нового екземпляра зі зміною окремих властивостей ([Program.cs](../03-RecordEqualityAndWith/Program.cs#L50)).
3. **Метод `Deconstruct`**: розпакування властивостей у кортеж змінних ([Program.cs](../03-RecordEqualityAndWith/Program.cs#L55)).
