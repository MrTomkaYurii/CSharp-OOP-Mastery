namespace GenericsPitfalls.Infrastructure;

// Неузагальнений базовий клас: його статичне поле ОДНЕ на всю програму.
public class EntityCounterBase
{
    public static int Total;
}

// Кожен закритий тип (EntityCounter<Dog>, EntityCounter<Cat>, ...) має ВЛАСНУ копію
// статичних полів, оголошених у самому EntityCounter<T>.
public class EntityCounter<T> : EntityCounterBase
{
    public static int Count;

    public EntityCounter()
    {
        Count++;      // окремий лічильник для кожного T
        Total++;      // спільний лічильник (поле базового класу)
    }
}
