# Hana Media Encoder 
Hana Media Encoder is the multimedia application that have feature to encode, decode, trim and muxing media files using FFMPEG pre-build application (Windows Version),
Hana Media Encoder works by creating line of command from user preferences (From application features) then execute it to FFMPEG, FFPLAY or FFPROBE.
This application are focusing on encoding video file by using only hardware GPU accelerated codec that FFMPEG has support it, for now (H264 and H265) codec only,
and it doesn't have any aim to support native / software or CPU based encoding for video in the future. For audio codec it only supported for MP3, FLAC and WAV,
and it will add other codec later.

# Current Features:
* Support multiple audio or video stream with same or different profile for each stream
* Support mux/demux media file with specific stream or all stream and with same or different profile for each stream
* Support trim media file with specific stream or all stream and with same or different profile for each stream
* Support preview media file by using FFPLAY

# Current Supported Video Codec
- H264 / H265 (AVC / HEVC)
* H265 / HEVC (Native codec are not supported yet)
* H264_QSV / HEVC_QSV (Intel Quicksync H.264/HEVC Encoder)
* H264_AMF / HEVC_AMF (AMD AMF H.264/HEVC Encoder)
* H264_NVENC / HEVC_NVEBC (Nvidia H.264/HEVC Encoder)

# Current Supported Video Codec
* MP3 (libmp3lame | ffmpeg codec)
* FLAC (flac | ffmpeg codec)
* WAV (libpcm_s16le, libpcm_s24le, libpcm_s32le)

# How to install
* Download .NET Framework 6.0, Hana Media Encoder setup & FFMPEG Pre-build
* Extract FFMPEG to folder
* Install Hana Media Encoder with folder location except "Program Files" or "Program Files (x86)", better to install it on desktop or other folder that doesn't required any permission (Due write-access protection or run this application as administrator if want to install in that directory
* Run Hana Media Encoder
* Go to options menu
* Configure FFMPEG binary folder to your bin folder from FFMPEG folder (FFMPEG/bin)
* After application restart then open application again then go to options
* Checklist 'Enable GPU Hardware Accelerated'
* And You're good to go

# App Compatibility
- [.NET Desktop Runtime 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [FFMPEG](https://www.gyan.dev/ffmpeg/builds/)

# HANA-CI Build Project 2016 - 2022