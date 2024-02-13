namespace Hadron.Tasks;

public partial class TaskUtils {
    /// <summary>
    /// Waits for the completion of a single task.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to wait for.</param>
    /// <returns>The completed task.</returns>
    public static Task<T> WhenAll<T>(Task<T> task) {
        return task;
    }

    /// <summary>
    /// Waits for the completion of two tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <returns>A tuple containing the results of the two tasks.</returns>
    public static async Task<(T1, T2)> WhenAll<T1, T2>(Task<T1> task1, Task<T2> task2) {
        await Task.WhenAll(task1, task2);
        return (task1.Result, task2.Result);
    }

    /// <summary>
    /// Waits for the completion of three tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <returns>A tuple containing the results of the three tasks.</returns>
    public static async Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3) {
        await Task.WhenAll(task1, task2, task3);
        return (task1.Result, task2.Result, task3.Result);
    }

    /// <summary>
    /// Waits for the completion of four tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <returns>A tuple containing the results of the four tasks.</returns>
    public static async Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4) {
        await Task.WhenAll(task1, task2, task3, task4);
        return (task1.Result, task2.Result, task3.Result, task4.Result);
    }

    /// <summary>
    /// Waits for the completion of five tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <typeparam name="T5">The type of the fifth task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <param name="task5">The fifth task to wait for.</param>
    /// <returns>A tuple containing the results of the five tasks.</returns>
    public static async Task<(T1, T2, T3, T4, T5)> WhenAll<T1, T2, T3, T4, T5>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5) {
        await Task.WhenAll(task1, task2, task3, task4, task5);
        return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result);
    }

    /// <summary>
    /// Waits for the completion of six tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <typeparam name="T5">The type of the fifth task result.</typeparam>
    /// <typeparam name="T6">The type of the sixth task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <param name="task5">The fifth task to wait for.</param>
    /// <param name="task6">The sixth task to wait for.</param>
    /// <returns>A tuple containing the results of the six tasks.</returns>
    public static async Task<(T1, T2, T3, T4, T5, T6)> WhenAll<T1, T2, T3, T4, T5, T6>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6) {
        await Task.WhenAll(task1, task2, task3, task4, task5, task6);
        return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result);
    }

    /// <summary>
    /// Waits for the completion of seven tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <typeparam name="T5">The type of the fifth task result.</typeparam>
    /// <typeparam name="T6">The type of the sixth task result.</typeparam>
    /// <typeparam name="T7">The type of the seventh task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <param name="task5">The fifth task to wait for.</param>
    /// <param name="task6">The sixth task to wait for.</param>
    /// <param name="task7">The seventh task to wait for.</param>
    /// <returns>A tuple containing the results of the seven tasks.</returns>
    public static async Task<(T1, T2, T3, T4, T5, T6, T7)> WhenAll<T1, T2, T3, T4, T5, T6, T7>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7) {
        await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);
        return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result);
    }

    /// <summary>
    /// Waits for the completion of eight tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <typeparam name="T5">The type of the fifth task result.</typeparam>
    /// <typeparam name="T6">The type of the sixth task result.</typeparam>
    /// <typeparam name="T7">The type of the seventh task result.</typeparam>
    /// <typeparam name="T8">The type of the eighth task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <param name="task5">The fifth task to wait for.</param>
    /// <param name="task6">The sixth task to wait for.</param>
    /// <param name="task7">The seventh task to wait for.</param>
    /// <param name="task8">The eighth task to wait for.</param>
    /// <returns>A tuple containing the results of the eight tasks.</returns>
    public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7, Task<T8> task8) {
        await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8);
        return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result);
    }

    /// <summary>
    /// Waits for the completion of nine tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <typeparam name="T5">The type of the fifth task result.</typeparam>
    /// <typeparam name="T6">The type of the sixth task result.</typeparam>
    /// <typeparam name="T7">The type of the seventh task result.</typeparam>
    /// <typeparam name="T8">The type of the eighth task result.</typeparam>
    /// <typeparam name="T9">The type of the ninth task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <param name="task5">The fifth task to wait for.</param>
    /// <param name="task6">The sixth task to wait for.</param>
    /// <param name="task7">The seventh task to wait for.</param>
    /// <param name="task8">The eighth task to wait for.</param>
    /// <param name="task9">The ninth task to wait for.</param>
    /// <returns>A tuple containing the results of the nine tasks.</returns>
    public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7, Task<T8> task8, Task<T9> task9) {
        await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9);
        return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result);
    }

    /// <summary>
    /// Waits for the completion of ten tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <typeparam name="T5">The type of the fifth task result.</typeparam>
    /// <typeparam name="T6">The type of the sixth task result.</typeparam>
    /// <typeparam name="T7">The type of the seventh task result.</typeparam>
    /// <typeparam name="T8">The type of the eighth task result.</typeparam>
    /// <typeparam name="T9">The type of the ninth task result.</typeparam>
    /// <typeparam name="T10">The type of the tenth task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <param name="task5">The fifth task to wait for.</param>
    /// <param name="task6">The sixth task to wait for.</param>
    /// <param name="task7">The seventh task to wait for.</param>
    /// <param name="task8">The eighth task to wait for.</param>
    /// <param name="task9">The ninth task to wait for.</param>
    /// <param name="task10">The tenth task to wait for.</param>
    /// <returns>A tuple containing the results of the ten tasks.</returns>
    public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7, Task<T8> task8, Task<T9> task9, Task<T10> task10) {
        await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10);
        return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result);
    }

    /// <summary>
    /// Waits for the completion of eleven tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <typeparam name="T5">The type of the fifth task result.</typeparam>
    /// <typeparam name="T6">The type of the sixth task result.</typeparam>
    /// <typeparam name="T7">The type of the seventh task result.</typeparam>
    /// <typeparam name="T8">The type of the eighth task result.</typeparam>
    /// <typeparam name="T9">The type of the ninth task result.</typeparam>
    /// <typeparam name="T10">The type of the tenth task result.</typeparam>
    /// <typeparam name="T11">The type of the eleventh task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <param name="task5">The fifth task to wait for.</param>
    /// <param name="task6">The sixth task to wait for.</param>
    /// <param name="task7">The seventh task to wait for.</param>
    /// <param name="task8">The eighth task to wait for.</param>
    /// <param name="task9">The ninth task to wait for.</param>
    /// <param name="task10">The tenth task to wait for.</param>
    /// <param name="task11">The eleventh task to wait for.</param>
    /// <returns>A tuple containing the results of the eleven tasks.</returns>
    public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7, Task<T8> task8, Task<T9> task9, Task<T10> task10, Task<T11> task11) {
        await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11);
        return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result, task11.Result);
    }

    /// <summary>
    /// Waits for the completion of twelve tasks.
    /// </summary>
    /// <typeparam name="T1">The type of the first task result.</typeparam>
    /// <typeparam name="T2">The type of the second task result.</typeparam>
    /// <typeparam name="T3">The type of the third task result.</typeparam>
    /// <typeparam name="T4">The type of the fourth task result.</typeparam>
    /// <typeparam name="T5">The type of the fifth task result.</typeparam>
    /// <typeparam name="T6">The type of the sixth task result.</typeparam>
    /// <typeparam name="T7">The type of the seventh task result.</typeparam>
    /// <typeparam name="T8">The type of the eighth task result.</typeparam>
    /// <typeparam name="T9">The type of the ninth task result.</typeparam>
    /// <typeparam name="T10">The type of the tenth task result.</typeparam>
    /// <typeparam name="T11">The type of the eleventh task result.</typeparam>
    /// <typeparam name="T12">The type of the twelfth task result.</typeparam>
    /// <param name="task1">The first task to wait for.</param>
    /// <param name="task2">The second task to wait for.</param>
    /// <param name="task3">The third task to wait for.</param>
    /// <param name="task4">The fourth task to wait for.</param>
    /// <param name="task5">The fifth task to wait for.</param>
    /// <param name="task6">The sixth task to wait for.</param>
    /// <param name="task7">The seventh task to wait for.</param>
    /// <param name="task8">The eighth task to wait for.</param>
    /// <param name="task9">The ninth task to wait for.</param>
    /// <param name="task10">The tenth task to wait for.</param>
    /// <param name="task11">The eleventh task to wait for.</param>
    /// <param name="task12">The twelfth task to wait for.</param>
    /// <returns>A tuple containing the results of the twelve tasks.</returns>
    public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7, Task<T8> task8, Task<T9> task9, Task<T10> task10, Task<T11> task11, Task<T12> task12) {
        await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12);
        return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result, task9.Result, task10.Result, task11.Result, task12.Result);
    }
}
