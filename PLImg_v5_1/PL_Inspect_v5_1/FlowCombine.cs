﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV_Extension;
using System.Drawing;
using System.Threading;

namespace PLImaging
{
    public class FlowCombine
    {
        static Flow FL = new Flow();
        public Dictionary<ScanConfig,Func<byte[,,],Task<byte[,,]>>> PreProcMethodList;


        public event TrsBuffData evtScanedImage
        { add { FL.evtTrsScanedImage += value; } remove { } }

        public FlowCombine()
        {
            ProcMethodRegist();
        }
        

        public async Task EpiScanAsync( ScanConfig config )
        {
            await FL.MoveStartOrigin( config , 0 );
            bool? result;
            switch ( config )
            {
                case ScanConfig.Trigger_1:
                    result = await FL.ScanMomenet( config , 4 );
                    break;

                case ScanConfig.Trigger_2:
                    result =await FL.ScanMomenet( config , 4 );
                    break;

                case ScanConfig.Trigger_4:
                    result = await await FL.ScanMomenet( config , 4 )
                                .ContinueWith( x => FL.MoveStartOrigin( config , 1 ) , TaskContinuationOptions.ExecuteSynchronously )
                                .ContinueWith( x => FL.ScanMomenet( config , 4 )     , TaskContinuationOptions.ExecuteSynchronously )
                                .ContinueWith( x => FL.MoveStartOrigin( config , 2 ) , TaskContinuationOptions.ExecuteSynchronously )
                                .ContinueWith( x => FL.ScanMomenet( config , 4 )     , TaskContinuationOptions.ExecuteSynchronously )
                                .ContinueWith( x => FL.MoveStartOrigin( config , 3 ) , TaskContinuationOptions.ExecuteSynchronously );
                    break;
            }
        }

        #region Step1
        public Task<byte[,,]> PreProcessingAsync( byte[,,] input , ScanConfig config )
        {
            return PreProcMethodList [ config ](input);
        }

        public Task<List<System.Drawing.Rectangle>> GetBoxsAsync( byte [ , , ] input , ScanConfig config)
        {
            int areaup = 0;
            int areadw = 0;
            // insert config => areaup de
            return FL.GetBoxList(input , areaup , areadw );
        }

        public Task<Point[][]> GetContoursAsync( byte [ , , ] input , ScanConfig config )
        {
            int areaup = 0;
            int areadw = 0;
            // insert config => areaup de
            return FL.GetContourList( input , areaup , areadw );
        }

        public Task<byte[,,]> ResizeAsync( byte [ , , ] input , ScanConfig config )
        {
            return FL.ReszieImage( input , config.ToResizeH() , config.ToResizeW() );
        }

        public Task<byte [ , , ]> ToBgrArrayAsync( byte [ , , ] input )
        {
            return Task.Run( () => input.Gray2Bgr() );
        }
        
        public Task DrawCircles( Image<Bgr , byte> dst , List<DefectRawData> defectlist , int iterNum )
        {
            return Task.Run( () =>
            {
                foreach ( var df in defectlist )
                {
                    MCvScalar color;
                    color = new MCvScalar( 0 , 0 , 255 );
                    CvInvoke.Circle( dst
                                    , new System.Drawing.Point( ( int )df.CenterX , ( int )df.CenterY )
                                    , ( int )df.Radius
                                    , color
                                    , 2 );
                }
            } );
        }
        #endregion

        #region AfterScanProcessing
        public async Task<byte[,,]> CombineImageAsync( byte[][,,] imgs , int h , int w )
        {
            LinkedList<Task> tasklist = new LinkedList<Task>();

            imgs.ActLoop( img => tasklist.AddLast( FL.ReszieImage( img , h , w ) ) );
            await Task.WhenAll( tasklist.ToArray() );
            return await FL.HStacks( imgs );
        }

        public Task<List<DefectRawData>> ShiftResult(List<DefectRawData>[] src)
        {
            return Task.Run( () =>
             {




                 return new List<DefectRawData>();
             } );
        }

        public Task<byte[,,]> CombineBoxImg(byte[][,,] src)
        {
            return Task.Run(() =>
            {
                var temp =  new byte[1,2,3];
                return temp;
            } );

        }


        #endregion  

        #region Preprocessing method
        protected void ProcMethodRegist()
        {
            PreProcMethodList = new Dictionary<ScanConfig , Func<byte [ , , ] , Task<byte [ , , ]>>>();
            PreProcMethodList.Add( ScanConfig.Trigger_1 , async input =>
            {
                return await FL.Threshold( input , 100 );
            } );

            PreProcMethodList.Add( ScanConfig.Trigger_2 , async input =>
            {
                //

                return await FL.Threshold( input , 100 );
            } );
            PreProcMethodList.Add( ScanConfig.Trigger_4 , async input =>
            {
                //

                return await FL.Threshold( input , 100 );
            } );
        }

        public void LoadConfigFile()
        { }



        #endregion
    }


 

}

