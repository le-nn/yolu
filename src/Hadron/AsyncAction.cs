namespace Hadron;

/// <summary>
/// Represents an asynchronous action with no parameters.
/// </summary>
public delegate Task AsyncAction();

/// <summary>
/// Represents an asynchronous action with one parameter.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
public delegate Task AsyncAction<in T1>(T1 arg1);

/// <summary>
/// Represents an asynchronous action with two parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
public delegate Task AsyncAction<in T1, in T2>(T1 arg1, T2 arg2);

/// <summary>
/// Represents an asynchronous action with three parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);

/// <summary>
/// Represents an asynchronous action with four parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

/// <summary>
/// Represents an asynchronous action with five parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
/// <param name="arg5">The fifth parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

/// <summary>
/// Represents an asynchronous action with six parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
/// <param name="arg5">The fifth parameter.</param>
/// <param name="arg6">The sixth parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

/// <summary>
/// Represents an asynchronous action with seven parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
/// <param name="arg5">The fifth parameter.</param>
/// <param name="arg6">The sixth parameter.</param>
/// <param name="arg7">The seventh parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

/// <summary>
/// Represents an asynchronous action with eight parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
/// <param name="arg5">The fifth parameter.</param>
/// <param name="arg6">The sixth parameter.</param>
/// <param name="arg7">The seventh parameter.</param>
/// <param name="arg8">The eighth parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

/// <summary>
/// Represents an asynchronous action with nine parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
/// <param name="arg5">The fifth parameter.</param>
/// <param name="arg6">The sixth parameter.</param>
/// <param name="arg7">The seventh parameter.</param>
/// <param name="arg8">The eighth parameter.</param>
/// <param name="arg9">The ninth parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

/// <summary>
/// Represents an asynchronous action with ten parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
/// <param name="arg5">The fifth parameter.</param>
/// <param name="arg6">The sixth parameter.</param>
/// <param name="arg7">The seventh parameter.</param>
/// <param name="arg8">The eighth parameter.</param>
/// <param name="arg9">The ninth parameter.</param>
/// <param name="arg10">The tenth parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

/// <summary>
/// Represents an asynchronous action with eleven parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
/// <param name="arg5">The fifth parameter.</param>
/// <param name="arg6">The sixth parameter.</param>
/// <param name="arg7">The seventh parameter.</param>
/// <param name="arg8">The eighth parameter.</param>
/// <param name="arg9">The ninth parameter.</param>
/// <param name="arg10">The tenth parameter.</param>
/// <param name="arg11">The eleventh parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

/// <summary>
/// Represents an asynchronous action with twelve parameters.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter.</typeparam>
/// <typeparam name="T5">The type of the fifth parameter.</typeparam>
/// <typeparam name="T6">The type of the sixth parameter.</typeparam>
/// <typeparam name="T7">The type of the seventh parameter.</typeparam>
/// <typeparam name="T8">The type of the eighth parameter.</typeparam>
/// <typeparam name="T9">The type of the ninth parameter.</typeparam>
/// <typeparam name="T10">The type of the tenth parameter.</typeparam>
/// <typeparam name="T11">The type of the eleventh parameter.</typeparam>
/// <typeparam name="T12">The type of the twelfth parameter.</typeparam>
/// <param name="arg1">The first parameter.</param>
/// <param name="arg2">The second parameter.</param>
/// <param name="arg3">The third parameter.</param>
/// <param name="arg4">The fourth parameter.</param>
/// <param name="arg5">The fifth parameter.</param>
/// <param name="arg6">The sixth parameter.</param>
/// <param name="arg7">The seventh parameter.</param>
/// <param name="arg8">The eighth parameter.</param>
/// <param name="arg9">The ninth parameter.</param>
/// <param name="arg10">The tenth parameter.</param>
/// <param name="arg11">The eleventh parameter.</param>
/// <param name="arg12">The twelfth parameter.</param>
public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
