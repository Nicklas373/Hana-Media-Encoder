Imports Newtonsoft.Json.Linq
Imports System.Net
Module GlobalVariable
    ' Readonly variable
    Public ReadOnly AudioStreamFlagsPath As String = My.Application.Info.DirectoryPath & "\audioStream\"
    Public ReadOnly AudioStreamConfigPath As String = My.Application.Info.DirectoryPath & "\audioConfig\"
    Public ReadOnly AudioQueueFlagsPath As String = My.Application.Info.DirectoryPath & "\queue\audio\audioStream\"
    Public ReadOnly AudioQueueConfigPath As String = My.Application.Info.DirectoryPath & "\queue\audio\audioConfig\"
    Public ReadOnly ChapterStreamConfigPath As String = My.Application.Info.DirectoryPath & "\chapterConfig\"
    Public ReadOnly DownBtnPath As String = My.Application.Info.DirectoryPath & "\Assets\arrow_down.png"
    Public ReadOnly HMEEngine As String = My.Application.Info.DirectoryPath & "\HME-Engine\"
    Public ReadOnly HMESetTheme As String = My.Application.Info.DirectoryPath & "\Assets\theme.xml"
    Public ReadOnly NotifyIcoPath As String = My.Application.Info.DirectoryPath & "\Assets\HME_256.ico"
    Public ReadOnly MuxStreamConfigPath As String = My.Application.Info.DirectoryPath & "\muxConfig\"
    Public ReadOnly SpectrumPlaceholder As String = My.Application.Info.DirectoryPath & "\Assets\Spectrum_Placeholder.png"
    Public ReadOnly SpectrumErrorPlaceholder As String = My.Application.Info.DirectoryPath & "\Assets\Spectrum_Error_Placeholder.png"
    Public ReadOnly UpBtnPath As String = My.Application.Info.DirectoryPath & "\Assets\arrow_up.png"
    Public ReadOnly TrimStreamConfigPath As String = My.Application.Info.DirectoryPath & "\trimConfig\"
    Public ReadOnly VideoStreamFlagsPath As String = My.Application.Info.DirectoryPath & "\videoStream\"
    Public ReadOnly VideoStreamConfigPath As String = My.Application.Info.DirectoryPath & "\videoConfig\"
    Public ReadOnly VideoQueueFlagsPath As String = My.Application.Info.DirectoryPath & "\queue\video\videoStream\"
    Public ReadOnly VideoQueueConfigPath As String = My.Application.Info.DirectoryPath & "\queue\video\videoConfig\"
    Public ReadOnly VideoPlaceholder As String = My.Application.Info.DirectoryPath & "\Assets\Snapshot_Placeholder.png"
    Public ReadOnly VideoErrorPlaceholder As String = My.Application.Info.DirectoryPath & "\Assets\Snapshot_Error_Placeholder.png"

    ' Encode variable
    Public ChapterFlags As String
    Public MuxFlags As String
    Public TrimFlags As String

    ' Global variable
    Public AdditionalEncodeStats As String
    Public AddEncConf As String
    Public AddEncTrimConf As String
    Public AddEncPassConf As String
    Public AltEncodeStats As String
    Public AltEncodeConf As String
    Public AltEncodeTrimConf As String
    Public AlwaysFullscreenStats As String
    Public AlwaysFullscreenConf As String
    Public AltAlwaysFullscreenConf As String
    Public AppChangelog
    Public AppRel As String
    Public AppVer As String
    Public AudiostreamConfig As String
    Public AudiostreamFlags As String
    Public AudioStreamSourceList As String
    Public AudioQueue As Boolean
    Public AudioQueueCodecInf As String
    Public AudioQueueFlags As String
    Public AudioTEMPFileNameOpt As String
    Public AudioTEMPPreValue As String
    Public AudioTEMPPostValue As String
    Public AudioTEMPFormatOpt As String
    Public AudioTEMPSmpRate As String
    Public AudioTEMPCnvRatio As String
    Public AudioTEMPChn As String
    Public AudioTEMPChnMapping As String
    Public AudioTEMPBitDepth As String
    Public AudioTEMPQuickFlags As String
    Public AudioTEMPChkDir As String
    Public AudioTEMPNewSmpType As String
    Public audioTEMPProfile As String
    Public AspectRatio As String
    Public BitRate As String
    Public CurParsedVer As String()
    Public CurPos As Integer
    Public ColorRange As String
    Public DebugMode As String
    Public DownloadURL As String
    Public EncStartTime As DateTime
    Public EncEndTime As DateTime
    Public EncPass1 As Boolean
    Public encpass2 As Boolean = True
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
    Public HitReset As Boolean
    Public HwAccelFormat As String
    Public HwAccelDev As String
    Public Hwdefconfig As String
    Public ImageDir As String
    Public LastValue As Integer
    Public LastPBValue As Integer
    Public Newdebugmode As String
    Public Newdebugstate As String
    Public Newffargs As String
    Public Newffargs2 As String
    Public Newffres As String
    Public Newframestate As String
    Public NewParsedVer As String()
    Public MaxPos As Integer
    Public MaxBitRate As String
    Public MediaTEMPFolderLocation As String
    Public MediaQueueOrigDir As New List(Of String)
    Public MergedCurVer As Integer
    Public MergedNewVer As Integer
    Public MissedFlags(255) As Integer
    Public OpenFileDialog As New OpenFileDialog
    Public OpenFolderDialog As New FolderBrowserDialog
    Public OldTitle As String
    Public ReturnAudioStats As Boolean
    Public ReturnVideoStats As Boolean
    Public SaveFileDialog As New SaveFileDialog
    Public ScaleAlgo As String
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
    Public VideoAutoAsp As Boolean
    Public VideoAspValue As String
    Public VideoRes As String
    Public VideoScaleType As String
    Public VideoStreamConfig As String
    Public VideoStreamFlags As String
    Public VideoStreamSourceList As String
    Public VideoTEMPFileNameOpt As String
    Public VideoTEMPPreValue As String
    Public VideoTEMPPostValue As String
    Public VideoTEMPFormatOpt As String
    Public VideoQueue As Boolean
    Public VideoQueueFlags As String
    Public VideoQueueCodecInf As String
    Public WC As New WebClient
End Module