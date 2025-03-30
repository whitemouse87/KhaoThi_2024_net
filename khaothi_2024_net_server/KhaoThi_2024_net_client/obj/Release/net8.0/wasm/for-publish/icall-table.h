#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
161,
173,
174,
175,
176,
177,
178,
179,
180,
181,
184,
185,
186,
361,
362,
363,
393,
394,
395,
415,
416,
417,
418,
535,
536,
537,
540,
578,
579,
580,
581,
582,
587,
589,
591,
593,
598,
606,
607,
608,
609,
610,
611,
612,
613,
614,
615,
616,
617,
618,
619,
620,
621,
622,
624,
625,
626,
627,
628,
629,
630,
723,
724,
725,
726,
727,
728,
729,
730,
731,
732,
733,
734,
735,
736,
737,
738,
739,
741,
742,
743,
744,
745,
746,
747,
804,
815,
816,
883,
890,
893,
895,
900,
901,
903,
904,
908,
909,
911,
913,
914,
917,
918,
919,
922,
924,
927,
929,
931,
940,
1007,
1009,
1011,
1021,
1022,
1023,
1024,
1026,
1033,
1034,
1035,
1036,
1037,
1045,
1046,
1047,
1051,
1052,
1054,
1058,
1059,
1060,
1344,
1525,
1541,
1542,
9391,
9392,
9394,
9395,
9396,
9397,
9398,
9400,
9402,
9404,
9405,
9416,
9418,
9425,
9427,
9429,
9431,
9482,
9483,
9485,
9486,
9487,
9488,
9489,
9491,
9493,
10673,
10677,
10679,
10680,
10681,
10682,
10956,
10957,
10958,
10959,
10979,
10980,
10981,
10983,
11093,
11095,
11097,
11106,
11107,
11108,
11109,
11586,
11587,
11588,
11593,
11594,
11634,
11635,
11655,
11662,
11669,
11680,
11684,
11711,
11736,
11792,
11794,
11810,
11812,
11813,
11814,
11815,
11816,
11823,
11838,
11858,
11859,
11869,
11871,
11878,
11879,
11882,
11884,
11889,
11895,
11896,
11903,
11905,
11917,
11920,
11921,
11922,
11933,
11942,
11948,
11949,
11950,
11952,
11953,
11970,
11972,
11986,
12008,
12009,
12010,
12035,
12040,
12070,
12071,
12601,
12602,
12603,
12631,
12645,
12740,
12741,
12960,
12961,
12969,
12970,
12971,
12977,
13079,
13645,
13646,
14118,
14123,
14133,
15105,
15126,
15128,
15130,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal (int);
int ves_icall_System_Array_IsValueOfElementTypeInternal (int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy (int,int,int,int,int);
int ves_icall_System_Array_GetLengthInternal_raw (int,int,int);
int ves_icall_System_Array_GetLowerBoundInternal_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
void ves_icall_System_Array_GetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_InitializeInternal_raw (int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
void ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
void ves_icall_System_Enum_InternalBoxEnum_raw (int,int,int64_t,int);
int ves_icall_System_Enum_InternalGetCorElementType (int);
void ves_icall_System_Enum_InternalGetUnderlyingType_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
void ves_icall_System_GC_AddPressure (uint64_t);
void ves_icall_System_GC_RemovePressure (uint64_t);
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Acos (double);
double ves_icall_System_Math_Acosh (double);
double ves_icall_System_Math_Asin (double);
double ves_icall_System_Math_Asinh (double);
double ves_icall_System_Math_Atan (double);
double ves_icall_System_Math_Atan2 (double,double);
double ves_icall_System_Math_Atanh (double);
double ves_icall_System_Math_Cbrt (double);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Cosh (double);
double ves_icall_System_Math_Exp (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Log10 (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sinh (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_Tanh (double);
double ves_icall_System_Math_FusedMultiplyAdd (double,double,double);
double ves_icall_System_Math_Log2 (double);
double ves_icall_System_Math_ModF (double,int);
float ves_icall_System_MathF_Acos (float);
float ves_icall_System_MathF_Acosh (float);
float ves_icall_System_MathF_Asin (float);
float ves_icall_System_MathF_Asinh (float);
float ves_icall_System_MathF_Atan (float);
float ves_icall_System_MathF_Atan2 (float,float);
float ves_icall_System_MathF_Atanh (float);
float ves_icall_System_MathF_Cbrt (float);
float ves_icall_System_MathF_Ceiling (float);
float ves_icall_System_MathF_Cos (float);
float ves_icall_System_MathF_Cosh (float);
float ves_icall_System_MathF_Exp (float);
float ves_icall_System_MathF_Floor (float);
float ves_icall_System_MathF_Log (float);
float ves_icall_System_MathF_Log10 (float);
float ves_icall_System_MathF_Pow (float,float);
float ves_icall_System_MathF_Sin (float);
float ves_icall_System_MathF_Sinh (float);
float ves_icall_System_MathF_Sqrt (float);
float ves_icall_System_MathF_Tan (float);
float ves_icall_System_MathF_Tanh (float);
float ves_icall_System_MathF_FusedMultiplyAdd (float,float,float);
float ves_icall_System_MathF_Log2 (float);
float ves_icall_System_MathF_ModF (float,int);
void ves_icall_System_RuntimeFieldHandle_SetValueDirect_raw (int,int,int,int,int,int);
void ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw (int,int,int);
void ves_icall_RuntimeMethodHandle_ReboxToNullable_raw (int,int,int,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
void ves_icall_RuntimeType_make_array_type_raw (int,int,int,int);
void ves_icall_RuntimeType_make_byref_type_raw (int,int,int);
void ves_icall_RuntimeType_make_pointer_type_raw (int,int,int);
void ves_icall_RuntimeType_MakeGenericType_raw (int,int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
int ves_icall_System_RuntimeType_CreateInstanceInternal_raw (int,int);
void ves_icall_System_RuntimeType_AllocateValueType_raw (int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringMethod_raw (int,int,int);
void ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetGenericArgumentsInternal_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition (int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetInterfaces_raw (int,int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringType_raw (int,int,int);
void ves_icall_RuntimeType_GetName_raw (int,int,int);
void ves_icall_RuntimeType_GetNamespace_raw (int,int,int);
int ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes (int);
int ves_icall_RuntimeTypeHandle_GetMetadataToken_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType (int);
int ves_icall_RuntimeTypeHandle_HasInstantiation (int);
int ves_icall_RuntimeTypeHandle_IsComObject_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetModule_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition (int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
void ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_String_InternalIsInterned_raw (int,int);
int ves_icall_System_String_InternalIntern_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
void ves_icall_System_TypedReference_InternalMakeTypedReference_raw (int,int,int,int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
int64_t ves_icall_System_Threading_Interlocked_Add_Long (int,int64_t);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw (int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw (int,int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw (int,int);
int ves_icall_System_Reflection_Assembly_GetCallingAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
void ves_icall_System_Reflection_AssemblyName_FreeAssemblyName (int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
int ves_icall_System_Reflection_LoaderAllocatorScout_Destroy (int);
int ves_icall_GetCurrentMethod_raw (int);
void ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInfoInternal_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw (int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetReferencedAssemblies_raw (int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
void ves_icall_InvokeClassConstructor_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw (int,int);
void ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw (int,int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
int ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw (int,int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_AssemblyExtensions_ApplyUpdateEnabled (int);
int ves_icall_AssemblyExtensions_GetApplyUpdateCapabilities_raw (int);
void ves_icall_AssemblyExtensions_ApplyUpdate (int,int,int,int,int,int,int);
int ves_icall_CustomAttributeBuilder_GetBlob_raw (int,int,int,int,int,int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int,int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
int ves_icall_System_Diagnostics_Debugger_IsAttached_internal ();
int ves_icall_System_Diagnostics_StackFrame_GetFrameInfo (int,int,int,int,int,int,int,int);
void ves_icall_System_Diagnostics_StackTrace_GetTrace (int,int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 161,
ves_icall_System_Array_InternalCreate,
// token 173,
ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal,
// token 174,
ves_icall_System_Array_IsValueOfElementTypeInternal,
// token 175,
ves_icall_System_Array_CanChangePrimitive,
// token 176,
ves_icall_System_Array_FastCopy,
// token 177,
ves_icall_System_Array_GetLengthInternal_raw,
// token 178,
ves_icall_System_Array_GetLowerBoundInternal_raw,
// token 179,
ves_icall_System_Array_GetGenericValue_icall,
// token 180,
ves_icall_System_Array_GetValueImpl_raw,
// token 181,
ves_icall_System_Array_SetGenericValue_icall,
// token 184,
ves_icall_System_Array_SetValueImpl_raw,
// token 185,
ves_icall_System_Array_InitializeInternal_raw,
// token 186,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 361,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 362,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 363,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 393,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 394,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 395,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 415,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 416,
ves_icall_System_Enum_InternalBoxEnum_raw,
// token 417,
ves_icall_System_Enum_InternalGetCorElementType,
// token 418,
ves_icall_System_Enum_InternalGetUnderlyingType_raw,
// token 535,
ves_icall_System_Environment_get_ProcessorCount,
// token 536,
ves_icall_System_Environment_get_TickCount,
// token 537,
ves_icall_System_Environment_get_TickCount64,
// token 540,
ves_icall_System_Environment_FailFast_raw,
// token 578,
ves_icall_System_GC_GetCollectionCount,
// token 579,
ves_icall_System_GC_AddPressure,
// token 580,
ves_icall_System_GC_RemovePressure,
// token 581,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 582,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 587,
ves_icall_System_GC_SuppressFinalize_raw,
// token 589,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 591,
ves_icall_System_GC_GetGCMemoryInfo,
// token 593,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 598,
ves_icall_System_Object_MemberwiseClone_raw,
// token 606,
ves_icall_System_Math_Acos,
// token 607,
ves_icall_System_Math_Acosh,
// token 608,
ves_icall_System_Math_Asin,
// token 609,
ves_icall_System_Math_Asinh,
// token 610,
ves_icall_System_Math_Atan,
// token 611,
ves_icall_System_Math_Atan2,
// token 612,
ves_icall_System_Math_Atanh,
// token 613,
ves_icall_System_Math_Cbrt,
// token 614,
ves_icall_System_Math_Ceiling,
// token 615,
ves_icall_System_Math_Cos,
// token 616,
ves_icall_System_Math_Cosh,
// token 617,
ves_icall_System_Math_Exp,
// token 618,
ves_icall_System_Math_Floor,
// token 619,
ves_icall_System_Math_Log,
// token 620,
ves_icall_System_Math_Log10,
// token 621,
ves_icall_System_Math_Pow,
// token 622,
ves_icall_System_Math_Sin,
// token 624,
ves_icall_System_Math_Sinh,
// token 625,
ves_icall_System_Math_Sqrt,
// token 626,
ves_icall_System_Math_Tan,
// token 627,
ves_icall_System_Math_Tanh,
// token 628,
ves_icall_System_Math_FusedMultiplyAdd,
// token 629,
ves_icall_System_Math_Log2,
// token 630,
ves_icall_System_Math_ModF,
// token 723,
ves_icall_System_MathF_Acos,
// token 724,
ves_icall_System_MathF_Acosh,
// token 725,
ves_icall_System_MathF_Asin,
// token 726,
ves_icall_System_MathF_Asinh,
// token 727,
ves_icall_System_MathF_Atan,
// token 728,
ves_icall_System_MathF_Atan2,
// token 729,
ves_icall_System_MathF_Atanh,
// token 730,
ves_icall_System_MathF_Cbrt,
// token 731,
ves_icall_System_MathF_Ceiling,
// token 732,
ves_icall_System_MathF_Cos,
// token 733,
ves_icall_System_MathF_Cosh,
// token 734,
ves_icall_System_MathF_Exp,
// token 735,
ves_icall_System_MathF_Floor,
// token 736,
ves_icall_System_MathF_Log,
// token 737,
ves_icall_System_MathF_Log10,
// token 738,
ves_icall_System_MathF_Pow,
// token 739,
ves_icall_System_MathF_Sin,
// token 741,
ves_icall_System_MathF_Sinh,
// token 742,
ves_icall_System_MathF_Sqrt,
// token 743,
ves_icall_System_MathF_Tan,
// token 744,
ves_icall_System_MathF_Tanh,
// token 745,
ves_icall_System_MathF_FusedMultiplyAdd,
// token 746,
ves_icall_System_MathF_Log2,
// token 747,
ves_icall_System_MathF_ModF,
// token 804,
ves_icall_System_RuntimeFieldHandle_SetValueDirect_raw,
// token 815,
ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw,
// token 816,
ves_icall_RuntimeMethodHandle_ReboxToNullable_raw,
// token 883,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 890,
ves_icall_RuntimeType_make_array_type_raw,
// token 893,
ves_icall_RuntimeType_make_byref_type_raw,
// token 895,
ves_icall_RuntimeType_make_pointer_type_raw,
// token 900,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 901,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 903,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 904,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 908,
ves_icall_System_RuntimeType_CreateInstanceInternal_raw,
// token 909,
ves_icall_System_RuntimeType_AllocateValueType_raw,
// token 911,
ves_icall_RuntimeType_GetDeclaringMethod_raw,
// token 913,
ves_icall_System_RuntimeType_getFullName_raw,
// token 914,
ves_icall_RuntimeType_GetGenericArgumentsInternal_raw,
// token 917,
ves_icall_RuntimeType_GetGenericParameterPosition,
// token 918,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 919,
ves_icall_RuntimeType_GetFields_native_raw,
// token 922,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 924,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 927,
ves_icall_RuntimeType_GetDeclaringType_raw,
// token 929,
ves_icall_RuntimeType_GetName_raw,
// token 931,
ves_icall_RuntimeType_GetNamespace_raw,
// token 940,
ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw,
// token 1007,
ves_icall_RuntimeTypeHandle_GetAttributes,
// token 1009,
ves_icall_RuntimeTypeHandle_GetMetadataToken_raw,
// token 1011,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 1021,
ves_icall_RuntimeTypeHandle_GetCorElementType,
// token 1022,
ves_icall_RuntimeTypeHandle_HasInstantiation,
// token 1023,
ves_icall_RuntimeTypeHandle_IsComObject_raw,
// token 1024,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 1026,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 1033,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 1034,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 1035,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 1036,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 1037,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 1045,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 1046,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition,
// token 1047,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 1051,
ves_icall_RuntimeTypeHandle_is_subclass_of_raw,
// token 1052,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 1054,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 1058,
ves_icall_System_String_FastAllocateString_raw,
// token 1059,
ves_icall_System_String_InternalIsInterned_raw,
// token 1060,
ves_icall_System_String_InternalIntern_raw,
// token 1344,
ves_icall_System_Type_internal_from_handle_raw,
// token 1525,
ves_icall_System_TypedReference_InternalMakeTypedReference_raw,
// token 1541,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1542,
ves_icall_System_ValueType_Equals_raw,
// token 9391,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 9392,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 9394,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 9395,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 9396,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 9397,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 9398,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 9400,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 9402,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 9404,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 9405,
ves_icall_System_Threading_Interlocked_Add_Long,
// token 9416,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 9418,
mono_monitor_exit_icall_raw,
// token 9425,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 9427,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 9429,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 9431,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 9482,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 9483,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 9485,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 9486,
ves_icall_System_Threading_Thread_GetState_raw,
// token 9487,
ves_icall_System_Threading_Thread_SetState_raw,
// token 9488,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 9489,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 9491,
ves_icall_System_Threading_Thread_YieldInternal,
// token 9493,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 10673,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 10677,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 10679,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 10680,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 10681,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 10682,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 10956,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 10957,
ves_icall_System_GCHandle_InternalFree_raw,
// token 10958,
ves_icall_System_GCHandle_InternalGet_raw,
// token 10959,
ves_icall_System_GCHandle_InternalSet_raw,
// token 10979,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 10980,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 10981,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 10983,
ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw,
// token 11093,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw,
// token 11095,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw,
// token 11097,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 11106,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 11107,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 11108,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw,
// token 11109,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 11586,
ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw,
// token 11587,
ves_icall_System_Reflection_Assembly_GetCallingAssembly_raw,
// token 11588,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 11593,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 11594,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 11634,
ves_icall_System_Reflection_AssemblyName_FreeAssemblyName,
// token 11635,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 11655,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 11662,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 11669,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 11680,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 11684,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 11711,
ves_icall_System_Reflection_LoaderAllocatorScout_Destroy,
// token 11736,
ves_icall_GetCurrentMethod_raw,
// token 11792,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 11794,
ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw,
// token 11810,
ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw,
// token 11812,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInfoInternal_raw,
// token 11813,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 11814,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 11815,
ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw,
// token 11816,
ves_icall_System_Reflection_Assembly_InternalGetReferencedAssemblies_raw,
// token 11823,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 11838,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 11858,
ves_icall_reflection_get_token_raw,
// token 11859,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 11869,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 11871,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 11878,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 11879,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 11882,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 11884,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 11889,
ves_icall_reflection_get_token_raw,
// token 11895,
ves_icall_get_method_info_raw,
// token 11896,
ves_icall_get_method_attributes,
// token 11903,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 11905,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 11917,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 11920,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 11921,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 11922,
ves_icall_reflection_get_token_raw,
// token 11933,
ves_icall_InternalInvoke_raw,
// token 11942,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 11948,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 11949,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 11950,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 11952,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 11953,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 11970,
ves_icall_InvokeClassConstructor_raw,
// token 11972,
ves_icall_InternalInvoke_raw,
// token 11986,
ves_icall_reflection_get_token_raw,
// token 12008,
ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw,
// token 12009,
ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw,
// token 12010,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 12035,
ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw,
// token 12040,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 12070,
ves_icall_reflection_get_token_raw,
// token 12071,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 12601,
ves_icall_AssemblyExtensions_ApplyUpdateEnabled,
// token 12602,
ves_icall_AssemblyExtensions_GetApplyUpdateCapabilities_raw,
// token 12603,
ves_icall_AssemblyExtensions_ApplyUpdate,
// token 12631,
ves_icall_CustomAttributeBuilder_GetBlob_raw,
// token 12645,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 12740,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 12741,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 12960,
ves_icall_ModuleBuilder_basic_init_raw,
// token 12961,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 12969,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 12970,
ves_icall_ModuleBuilder_getToken_raw,
// token 12971,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 12977,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 13079,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 13645,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 13646,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 14118,
ves_icall_System_Diagnostics_Debugger_IsAttached_internal,
// token 14123,
ves_icall_System_Diagnostics_StackFrame_GetFrameInfo,
// token 14133,
ves_icall_System_Diagnostics_StackTrace_GetTrace,
// token 15105,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 15126,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 15128,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 15130,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_flags [] = {
0,
0,
0,
0,
0,
4,
4,
0,
4,
0,
4,
4,
4,
0,
0,
0,
4,
4,
4,
4,
4,
0,
4,
0,
0,
0,
4,
0,
0,
0,
4,
4,
4,
4,
0,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
};
