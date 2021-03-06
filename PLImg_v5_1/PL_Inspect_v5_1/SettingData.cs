﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLImaging
{
    public enum ScanConfig { Trigger_1, Trigger_2, Trigger_4 }
    public enum StgBufferConfig { Trigger_1, Trigger_2, Trigger_4 }

    public class SettingData
    {
        public string CamPath;
        public string StgPath;
        public Dictionary<ScanConfig,double> StartYPos;
        public Dictionary<ScanConfig,double> StartXPos;
        public Dictionary<ScanConfig,double> EndYPos;
        public Dictionary<ScanConfig,ScanInfo> Info;

        public double XStep_Size       = 28.35;
        public double Scan_Stage_Speed = 17;
        public double Camera_Exposure  = 800;
        public double Camera_LineRate  = 1200;

        public SettingData() { CreateEndPoint(); }
        public SettingData(
            double xStep_Size = 28.2 ,
            double scan_Stage_Speed = 17 ,
            double camera_Exposure = 2400 ,
            double camera_LineRate = 400
            )
        {
            XStep_Size = xStep_Size;
            Scan_Stage_Speed = scan_Stage_Speed;
            Camera_Exposure = camera_Exposure;
            Camera_LineRate = camera_LineRate;

            CreateEndPoint();
        }

        void CreateEndPoint()
        {
            StartYPos = new Dictionary<ScanConfig , double>();
            StartYPos.Add( ScanConfig.Trigger_1 , 137 );
            StartYPos.Add( ScanConfig.Trigger_2 , 137 );
            StartYPos.Add( ScanConfig.Trigger_4 , 161 );

            StartXPos = new Dictionary<ScanConfig , double>();
            StartXPos.Add( ScanConfig.Trigger_1 , 60 );
            StartXPos.Add( ScanConfig.Trigger_2 , 72 );
            StartXPos.Add( ScanConfig.Trigger_4 , 20 );

            EndYPos = new Dictionary<ScanConfig , double>();
            EndYPos.Add( ScanConfig.Trigger_1 , 112 );
            EndYPos.Add( ScanConfig.Trigger_2 , 80 );
            EndYPos.Add( ScanConfig.Trigger_4 , 60 );


            Info = new Dictionary<ScanConfig , ScanInfo>();
            Info.Add( ScanConfig.Trigger_1 , new ScanInfo( 12000 , 24000 , 137 , 60 , 112 ) );
            Info.Add( ScanConfig.Trigger_2 , new ScanInfo( 12000 , 48000 , 137 , 72 , 80  ) );
            Info.Add( ScanConfig.Trigger_4 , new ScanInfo( 12000 , 96000 , 161 , 20 , 60  ) );
        }

    }

    public class ScanInfo
    {
        public int    BufferWidth;
        public int    BufferHeight;
        public double StartYPos;
        public double StartXPos;
        public double EndYPos;

        public ScanInfo( int bufW , int bufH , double starty , double startx , double endy )
        {
            BufferWidth = bufW;
            BufferHeight = bufH;
            StartYPos = starty;
            StartXPos = startx;
            EndYPos = endy;
        }

    }

}
