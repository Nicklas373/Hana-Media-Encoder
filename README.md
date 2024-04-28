<p align="center">
  <img width="300" height="300" src="https://github.com/Nicklas373/Hana-Media-Encoder/blob/master/Hana%20Media%20Encoder/Assets/HME_Logo.png"><br>
</p>

# Hana Media Encoder 
Hana Media Encoder is a open source multimedia application for FFMPEG or NVENCC that have feature to encode, decode, trim, mux or even batch processing.
It can detects and configure multiple media streams and can encode it separately if media file have more than one audio stream.
Hana Media Encoder works by creating line of command based on user preferences that was taken from application available menu or options
then execute it to FFMPEG, FFPLAY, FFPROBE or NVENCC.

# Current Features:
* Support multiple media file encoding with same or different profile for each stream
* Support mux/demux media file with specific stream or all stream and with same or different profile for each stream
* Support manage chapter from media file (Video Only)
* Support trim media file with specific stream or all stream and with same or different profile for each stream
* Support preview media file by using FFPLAY
* Support media queue / batch encoding for video and audio file

# Current Supported Video Output Codec
- H264 / HEVC / AV1 (Native codec are not supported yet)
- H264_QSV / HEVC_QSV / AV1_QSV (Intel Quicksync H.264/HEVC/AV1 Encoder)
- H264_AMF / HEVC_AMF / AV1_AMF (AMD AMF H.264/HEVC/AV1 Encoder)
- H264_NVENC / HEVC_NVENC / AV1_NVENC (Nvidia NVENC H.264/HEVC/AV1 Encoder)

# Current Supported Audio Output Codec
- MP3 (libmp3lame)
- FLAC (flac)
- WAV (libpcm)
- AAC (aac)
- OPUS (libopus)
- MP2 (libtwolame)

# How to install
* Download software requirement
	- [.NET Desktop Runtime 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
	- [FFMPEG](https://github.com/BtbN/FFmpeg-Builds/releases)
* Extract FFMPEG to folder
* Install Hana Media Encoder beside folder location except "Program Files" or "Program Files (x86)" or other folder that doesn't required any permission (Due write-access protection)
  (Ex: Install it on desktop or user folder)
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
- [FFMPEG](https://github.com/BtbN/FFmpeg-Builds/releases)
- [NVENCC](https://github.com/rigaya/NVEnc/releases)

# Documentation
<p align="left">
<img width="854" height="480" src="https://github.com/Nicklas373/Hana-Media-Encoder/blob/master/snap/snap_1.png">&nbsp;&nbsp;&nbsp;
<img width="854" height="480" src="https://github.com/Nicklas373/Hana-Media-Encoder/blob/master/snap/snap_2.png">&nbsp;&nbsp;&nbsp;
<img width="854" height="480" src="https://github.com/Nicklas373/Hana-Media-Encoder/blob/master/snap/snap_3.png">&nbsp;&nbsp;&nbsp;
</p>

# License
Copyright (C) 2016-2024 HANA-CI Build Project

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.

# HANA-CI Build Project 2016 - 2024