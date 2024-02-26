﻿namespace Yolu;

public class LibrarySettings {
    public static int StackallocThreshold {
        get {
            const string environmentVariableName = "DOTNEXT_STACK_ALLOC_THRESHOLD";
            const string configurationParameterName = "DotNext.Buffers.StackAllocThreshold";
            const int defaultValue = 511;
            const int minimumValue = 14;

            if (AppContext.GetData(configurationParameterName) is int result)
                goto exit;

            if (int.TryParse(Environment.GetEnvironmentVariable(environmentVariableName), out result))
                goto exit;

            exit:
            return result > minimumValue ? result : defaultValue;
        }
    }

    public static bool DisableRandomStringInternalBufferCleanup {
        get {
            const string switchName = "DotNext.Security.DisableRandomStringInternalBufferCleanup";
            const bool defaultValue = false;

            if (!AppContext.TryGetSwitch(switchName, out var result))
                result = defaultValue;

            return result;
        }
    }

    public static bool DisableNativeAllocation {
        get {
            const string switchName = "DotNext.Buffers.DisableNativeAllocation";
            const bool defaultValue = false;

            if (!AppContext.TryGetSwitch(switchName, out var result))
                result = defaultValue;

            return result;
        }
    }
}
