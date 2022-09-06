Imports Newtonsoft.Json.Linq
Imports System.Net
Module GlobalVariable
    ' Readonly variable
    Public ReadOnly AudioStreamFlagsPath As String = "audioStream\"
    Public ReadOnly AudioStreamConfigPath As String = "audioConfig\"
    Public ReadOnly SpectrumPlaceholder As String = "Assets\Spectrum_Placeholder.png"
    Public ReadOnly SpectrumErrorPlaceholder As String = "Assets\Spectrum_Error_Placeholder.png"
    Public ReadOnly VideoStreamFlagsPath As String = "videoStream\"
    Public ReadOnly VideoStreamConfigPath As String = "videoConfig\"
    Public ReadOnly VideoPlaceholder As String = "Assets\Snapshot_Placeholder.png"
    Public ReadOnly VideoErrorPlaceholder As String = "Assets\Snapshot_Error_Placeholder.png"

    ' Global variable
    Public AppChangelog
    Public AppRel As String
    Public AppVer As String
    Public AudiostreamConfig As String
    Public AudiostreamFlags As String
    Public AudioStreamSourceList As String
    Public AspectRatio As String
    Public BitRate As String
    Public CurParsedVer As String()
    Public CurPos As Integer
    Public ConfigState As Boolean
    Public DebugMode As String
    Public DownloadURL As String
    Public EncStartTime As DateTime
    Public EncEndTime As DateTime
    Public FfmpegConf As String
    Public FfmpegConfig As String
    Public FfmpegLetter As String
    Public FfmpegEncStats As String
    Public FfmpegErr As String
    Public FlagsAudioCount As Integer
    Public FlagsAudioValue As Integer
    Public FlagsCount As Integer
    Public FlagsResult As Integer
    Public FlagsStart As Integer
    Public FlagsValue As Integer
    Public FlagsVideoCount As Integer
    Public FlagsVideoValue As Integer
    Public FrameConfig As String
    Public FrameCount As String
    Public FrameMode As String
    Public FPS As String
    Public HwAccelFormat As String
    Public HwAccelDev As String
    Public Hwdefconfig As String
    Public ImageDir As String
    Public Newdebugmode As String
    Public Newdebugstate As String
    Public Newffargs As String
    Public Newffargs2 As String
    Public Newffres As String
    Public Newframestate As String
    Public NewParsedVer As String()
    Public MaxPos As Integer
    Public MaxBitRate As String
    Public MergedCurVer As Integer
    Public MergedNewVer As Integer
    Public MissedFlags(255) As Integer
    Public OpenFileDialog As New OpenFileDialog
    Public OpenFolderDialog As New FolderBrowserDialog
    Public ReturnAudioStats As Boolean
    Public ReturnVideoStats As Boolean
    Public SaveFileDialog As New SaveFileDialog
    Public StreamCount As Integer
    Public StreamInfo As String
    Public StreamStart As Integer
    Public OrigSavePath As String
    Public OrigSaveExt As String
    Public OrigSaveName As String
    Public OTA As String
    Public OriTime As Integer
    Public Parsedchangelog As String
    Public Parsejson As JObject
    Public ProgPercentage As Integer
    Public TargetQualityControl As String
    Public TempValue As String
    Public TimeSplit As String()
    Public TimeDur As Integer
    Public TimeChapter As Integer
    Public TrimCondition As Boolean
    Public TrimEndTime As Integer
    Public TrimStartTime As Integer
    Public TrimPreCondition As Integer
    Public TotalScreenshot As Integer
    Public TotalSpectrum As Integer
    Public Videofile As String
    Public VideoFilePath As String
    Public VideoHeight As String
    Public VideoWidth As String
    Public VideoStreamConfig As String
    Public VideoStreamFlags As String
    Public VideoStreamSourceList As String
    Public WC As New WebClient
End Module