Public Class ScannerIF

#Region "Constant and Variable declarations"

    Public Const DRIVER_DLL_NAME As String = "LLT.dll"

    Public Const CONVERT_X As Integer = &H800
    Public Const CONVERT_Z As Integer = &H1000



    'General return-values for all functions
    Public Const GENERAL_FUNCTION_DEVICE_NAME_NOT_SUPPORTED As Integer = 4
    Public Const GENERAL_FUNCTION_PACKET_SIZE_CHANGED As Integer = 3
    Public Const GENERAL_FUNCTION_CONTAINER_MODE_HEIGHT_CHANGED As Integer = 2
    Public Const GENERAL_FUNCTION_OK As Integer = 1
    Public Const GENERAL_FUNCTION_NOT_AVAILABLE As Integer = 0

    ' "Return Values"
    'Function defines for the Get/SetFeature function
    Public Const FEATURE_FUNCTION_SERIAL As Integer = &HF0000410
    Public Const FEATURE_FUNCTION_LASERPOWER As Integer = &HF0F00824
    Public Const INQUIRY_FUNCTION_LASERPOWER As Integer = &HF0F00524
    Public Const FEATURE_FUNCTION_MEASURINGFIELD As Integer = &HF0F00880
    Public Const INQUIRY_FUNCTION_MEASURINGFIELD As Integer = &HF0F00580
    Public Const FEATURE_FUNCTION_TRIGGER As Integer = &HF0F00830
    Public Const INQUIRY_FUNCTION_TRIGGER As Integer = &HF0F00530
    Public Const FEATURE_FUNCTION_SHUTTERTIME As Integer = &HF0F0081C
    Public Const INQUIRY_FUNCTION_SHUTTERTIME As Integer = &HF0F0051C
    Public Const FEATURE_FUNCTION_IDLETIME As Integer = &HF0F00800
    Public Const INQUIRY_FUNCTION_IDLETIME As Integer = &HF0F00500
    Public Const FEATURE_FUNCTION_PROCESSING_PROFILEDATA As Integer = &HF0F00804
    Public Const INQUIRY_FUNCTION_PROCESSING_PROFILEDATA As Integer = &HF0F00504
    Public Const FEATURE_FUNCTION_THRESHOLD As Integer = &HF0F00810
    Public Const INQUIRY_FUNCTION_THRESHOLD = &HF0F00510
    Public Const FEATURE_FUNCTION_MAINTENANCEFUNCTIONS As Integer = &HF0F0088C
    Public Const INQUIRY_FUNCTION_MAINTENANCEFUNCTIONS As Integer = &HF0F0058C
    Public Const FEATURE_FUNCTION_ANALOGFREQUENCY As Integer = &HF0F00828
    Public Const INQUIRY_FUNCTION_ANALOGFREQUENCY As Integer = &HF0F00528
    Public Const FEATURE_FUNCTION_ANALOGOUTPUTMODES As Integer = &HF0F00820
    Public Const INQUIRY_FUNCTION_ANALOGOUTPUTMODES As Integer = &HF0F00520
    Public Const FEATURE_FUNCTION_CMMTRIGGER As Integer = &HF0F00888
    Public Const INQUIRY_FUNCTION_CMMTRIGGER As Integer = &HF0F00588
    Public Const FEATURE_FUNCTION_REARRANGEMENT_PROFILE As Integer = &HF0F0080C
    Public Const INQUIRY_FUNCTION_REARRANGEMENT_PROFILE As Integer = &HF0F0050C
    Public Const FEATURE_FUNCTION_PROFILE_FILTER As Integer = &HF0F00818
    Public Const INQUIRY_FUNCTION_PROFILE_FILTER As Integer = &HF0F00518
    Public Const FEATURE_FUNCTION_RS422_INTERFACE_FUNCTION As Integer = &HF0F008C0
    Public Const INQUIRY_FUNCTION_RS422_INTERFACE_FUNCTION As Integer = &HF0F005C0

    Public Const FEATURE_FUNCTION_SATURATION As Integer = &HF0F00814
    Public Const INQUIRY_FUNCTION_SATURATION As Integer = &HF0F00514
    Public Const FEATURE_FUNCTION_TEMPERATURE As Integer = &HF0F0082C
    Public Const INQUIRY_FUNCTION_TEMPERATURE As Integer = &HF0F0052C
    Public Const FEATURE_FUNCTION_CAPTURE_QUALITY As Integer = &HF0F008C4
    Public Const INQUIRY_FUNCTION_CAPTURE_QUALITY As Integer = &HF0F005C4
    Public Const FEATURE_FUNCTION_SHARPNESS As Integer = &HF0F00808
    Public Const INQUIRY_FUNCTION_SHARPNESS As Integer = &HF0F00508


    '#Region "Enums"

    'specifies the interface type for CreateLLTDevice and IsInterfaceType
    Public Enum TInterfaceType

        INTF_TYPE_UNKNOWN = 0
        INTF_TYPE_SERIAL = 1
        INTF_TYPE_FIREWIRE = 2
        INTF_TYPE_ETERNET = 3
    End Enum

    'specify the callback type
    'if you programming language don't support enums, you can use a signed int
    Public Enum TCallbackType

        STD_CALL = 0
        C_DECL = 1
    End Enum

    'specify the type of the scanner
    'if you programming language don't support enums, you can use a signed int
    Public Enum TScannerType

        StandardType = -1                    'can't decode scanCONTROL name use standard measurmentrange
        LLT25 = 0                           'scanCONTROL28xx with 25mm measurmentrange
        LLT100 = 1                           'scanCONTROL28xx with 100mm measurmentrange
        scanCONTROL28xx_25 = 0              'scanCONTROL28xx with 25mm measurmentrange
        scanCONTROL28xx_100 = 1              'scanCONTROL28xx with 100mm measurmentrange
        scanCONTROL28xx_10 = 2              'scanCONTROL28xx with 10mm measurmentrange
        scanCONTROL28xx_xxx = 999            'scanCONTROL28xx with no measurmentrange -> use standard measurmentrange
        scanCONTROL27xx_25 = 1000           'scanCONTROL27xx with 25mm measurmentrange
        scanCONTROL27xx_100 = 1001           'scanCONTROL27xx with 100mm measurmentrange
        scanCONTROL27xx_50 = 1002           'scanCONTROL27xx with 50mm measurmentrange
        scanCONTROL2751_100 = 1021           'scanCONTROL27xx with 100mm measurmentrange
        scanCONTROL2702_50 = 1032           'scanCONTROL2702 with 50mm measurement range
        scanCONTROL27xx_xxx = 1999           'scanCONTROL27xx with no measurmentrange -> use standard measurmentrange

    End Enum
    'specify the profile configuration
    'if you programming language don't support enums, you can use a signed int
    Public Enum TProfileConfig

        NONE = 0
        PROFILE = 1
        CONTAINER = 1
        VIDEO_IMAGE = 1
        PURE_PROFILE = 2
        QUARTER_PROFILE = 3
        CSV_PROFILE = 4
        PARTIAL_PROFILE = 5
    End Enum


    'specify the type for the profile transfer
    'if you programming language don't support enums, you can use a signed int
    Public Enum TTransferProfileType

        NORMAL_TRANSFER = 0
        SHOT_TRANSFER = 1
        NORMAL_CONTAINER_MODE = 2
        SHOT_CONTAINER_MODE = 3
    End Enum

#End Region


#Region "DLL references"

    Public Declare Function s_CreateLLTDevice Lib "llt.dll" (ByVal InterfaceType As TInterfaceType) As Integer

    Public Declare Function s_GetDeviceInterfaces Lib "llt.dll" (ByVal pLLT As UInteger, ByVal pInterfaces As UInteger(), _
                                                                                                ByVal nSize As Integer) As Integer

    Public Declare Function s_Connect Lib "llt.dll" (ByVal pLLT As UInteger) As Integer

    Public Declare Function s_Disconnect Lib "llt.dll" (ByVal pLLT As UInteger) As Integer

    Public Declare Function s_GetDeviceInterfaces Lib "llt.dll" (ByVal pLLT As UInteger, ByVal pInterfaces As Integer, _
                                                                                                ByVal nSize As Integer) As Integer

    Public Declare Function s_SetDeviceInterface Lib "llt.dll" (ByVal pLLT As UInteger, ByVal nInterface As UInteger, _
                                                                                               ByVal nAdditional As Integer) As Integer

    Public Declare Function s_GetDeviceName Lib "llt.dll" (ByVal pLLT As UInteger, ByVal pDevName As Byte(), _
                                    ByVal nDevNameSize As Integer, ByVal pVendName As Byte, ByVal nVenNameSize As Integer) As Integer

    Public Declare Function s_GetLLTType Lib "llt.dll" (ByVal pLLT As UInteger, ByRef ScannerType As TScannerType) As Integer

    Public Declare Function s_TranslateErrorValue Lib "llt.dll" (ByVal pLLT As UInteger, ByVal ErrorValue As Integer, _
                                                                                                ByVal pString As Byte(), ByVal nStringSize As Integer) As Integer

    Public Declare Function s_GetResolutions Lib "llt.dll" (ByVal pLLT As UInteger, ByVal pValue As UInteger(), _
                                                                                           ByVal nSize As Integer) As Integer

    Public Declare Function s_GetFeature Lib "llt.dll" (ByVal pLLT As UInteger, ByVal Function1 As Integer, _
                                                                                           ByRef pValue As UInteger) As Integer

    Public Declare Function s_SetFeature Lib "llt.dll" (ByVal pLLT As UInteger, ByVal Function1 As Integer, _
                                                                                           ByVal Value As UInteger) As Integer

    Public Declare Function s_SetResolution Lib "llt.dll" (ByVal pLLT As UInteger, ByVal Value As UInteger) As Integer

    Public Declare Function s_SetProfileConfig Lib "llt.dll" (ByVal pLLT As UInteger, ByVal Value As TProfileConfig) As Integer

    Public Declare Function s_TransferProfiles Lib "llt.dll" (ByVal pLLT As UInteger, ByVal TransferProfileType As TTransferProfileType, _
                                                                                             ByVal nEnable As Integer) As Integer

    Public Declare Function s_ConvertProfile2Values Lib "llt.dll" (ByVal pLLT As UInteger, ByVal pProfile As Byte(), _
                            ByVal nResolution As UInteger, ByVal ProfileConfig As TProfileConfig, ByVal ScannerType As TScannerType, _
                            ByVal nReflection As UInteger, ByVal nConvertToMM As Integer, ByVal pWidth As UShort(), ByVal pMaximum As UShort(), _
                            ByVal pThreshold As UShort(), ByVal pX As Double(), ByVal pZ As Double(), ByVal pM0 As UInteger(), ByVal pM1 As UInteger()) As Integer

    Public Declare Function s_GetActualProfile Lib "llt.dll" (ByVal pLLT As UInteger, ByVal pBuffer As Byte(), _
                            ByVal nBuffersize As Integer, ByVal ProfileConfig As TProfileConfig, ByRef pLostProfiles As UInteger) As Integer


    Public Declare Function s_Timestamp2TimeAndCount Lib "llt.dll" (ByVal pBuffer As Byte(), ByRef dTimeShutterOpen As Double, _
                                                                ByRef dTimeShutterClose As Double, ByRef uiProfileCount As UInteger) As Integer


#End Region


End Class

