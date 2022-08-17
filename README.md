# Hana Media Encoder 
Hana Media Encoder is the multimedia application that have feature to encode, decode, many other function to process media files using FFMPEG pre-build application (Windows Version),
Hana Media Encoder works by creating line of command based on user preferences that was taken from application available menu or options then execute it to FFMPEG, FFPLAY or FFPROBE.
Hana Media Encoder only focus to work with only hardware GPU accelerated codec that FFMPEG has support it, for now (H264 and H265) codec only, and it does not have any aim to support
native or software or even CPU based encoding for media file in the future.

For audio codec it only supported for MP3, FLAC and WAV, and it will add other codec later.

# Current Features:
* Support multiple media file encoding with same or different profile for each stream
* Support mux/demux media file with specific stream or all stream and with same or different profile for each stream
* Support create chapter (Insert chapter METADATA) into media file (Video Only)
* Support trim media file with specific stream or all stream and with same or different profile for each stream
* Support preview media file by using FFPLAY

# Current Supported Video Codec
- H264 / H265 (AVC / HEVC)
* H264 / HEVC (Native codec are not supported yet)
* H264_QSV / HEVC_QSV (Intel Quicksync H.264/HEVC Encoder)
* H264_AMF / HEVC_AMF (AMD AMF H.264/HEVC Encoder)
* H264_NVENC / HEVC_NVENC (Nvidia H.264/HEVC Encoder)

# Current Supported Audio Codec
* MP3 (libmp3lame)
* FLAC (flac)
* WAV (libpcm)

# How to install
* Download .NET Framework 6.0, Hana Media Encoder setup & FFMPEG Pre-build
* Extract FFMPEG to folder
* Install Hana Media Encoder with folder location except "Program Files" or "Program Files (x86)", better to install it on desktop or other folder that doesn't required any permission (Due write-access protection or run this application as administrator if want to install in that directory
* Run Hana Media Encoder
* Go to options menu
* Configure FFMPEG binary folder to your bin folder from FFMPEG folder (FFMPEG/bin)
* After application restart then open application again then go to options
* Checklist 'Enable GPU Hardware Accelerated' [NOTE: Please configure correct GPU name with your current primary GPU Renderer]
* And You're good to go

# Additional library:
- [SyncFusion](https://www.syncfusion.com/) (WinForms)
- [Newtonsoft](https://www.newtonsoft.com/json) (Json.NET)

# App Compatibility
- [.NET Desktop Runtime 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [FFMPEG](https://www.gyan.dev/ffmpeg/builds/)

# HANA-CI Build Project 2016 - 2022