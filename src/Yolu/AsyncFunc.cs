namespace Yolu;

/// <summary>
/// Represents an asynchronous function that returns a task with a result of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
public delegate Task<T> AsyncFunc<T>();

/// <summary>
/// Represents an asynchronous function that takes one input parameter of type <typeparamref name="T1"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, TResult>(T1 arg1);

/// <summary>
/// Represents an asynchronous function that takes two input parameters of types <typeparamref name="T1"/> and <typeparamref name="T2"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, TResult>(T1 arg1, T2 arg2);

/// <summary>
/// Represents an asynchronous function that takes three input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, and <typeparamref name="T3"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, TResult>(T1 arg1, T2 arg2, T3 arg3);

/// <summary>
/// Represents an asynchronous function that takes four input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, and <typeparamref name="T4"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

/// <summary>
/// Represents an asynchronous function that takes five input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, and <typeparamref name="T5"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

/// <summary>
/// Represents an asynchronous function that takes six input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, and <typeparamref name="T6"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

/// <summary>
/// Represents an asynchronous function that takes seven input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>, and <typeparamref name="T7"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth input parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

/// <summary>
/// Represents an asynchronous function that takes eight input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>, <typeparamref name="T7"/>, and <typeparamref name="T8"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth input parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh input parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

/// <summary>
/// Represents an asynchronous function that takes nine input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>, <typeparamref name="T7"/>, <typeparamref name="T8"/>, and <typeparamref name="T9"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth input parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh input parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth input parameter.</typeparam>
/// <typeparam name="T9">The type of the ninth input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

/// <summary>
/// Represents an asynchronous function that takes ten input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>, <typeparamref name="T7"/>, <typeparamref name="T8"/>, <typeparamref name="T9"/>, and <typeparamref name="T10"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth input parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh input parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth input parameter.</typeparam>
/// <typeparam name="T9">The type of the ninth input parameter.</typeparam>
/// <typeparam name="T10">The type of the tenth input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

/// <summary>
/// Represents an asynchronous function that takes eleven input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>, <typeparamref name="T7"/>, <typeparamref name="T8"/>, <typeparamref name="T9"/>, <typeparamref name="T10"/>, and <typeparamref name="T11"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth input parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh input parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth input parameter.</typeparam>
/// <typeparam name="T9">The type of the ninth input parameter.</typeparam>
/// <typeparam name="T10">The type of the tenth input parameter.</typeparam>
/// <typeparam name="T11">The type of the eleventh input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

/// <summary>
/// Represents an asynchronous function that takes twelve input parameters of types <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>, <typeparamref name="T6"/>, <typeparamref name="T7"/>, <typeparamref name="T8"/>, <typeparamref name="T9"/>, <typeparamref name="T10"/>, <typeparamref name="T11"/>, and <typeparamref name="T12"/> and returns a task with a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T1">The type of the first input parameter.</typeparam>
/// <typeparam name="T2">The type of the second input parameter.</typeparam>
/// <typeparam name="T3">The type of the third input parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth input parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh input parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth input parameter.</typeparam>
/// <typeparam name="T9">The type of the ninth input parameter.</typeparam>
/// <typeparam name="T10">The type of the tenth input parameter.</typeparam>
/// <typeparam name="T11">The type of the eleventh input parameter.</typeparam>
/// <typeparam name="T12">The type of the twelfth input parameter.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
