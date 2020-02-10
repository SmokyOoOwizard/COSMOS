using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COSMOS.Shaders
{
    public class GaussianBlurStatic
    {
        ComputeShader blurShader;

        int HorId;
        int VerId;

        RenderTexture horBlurOutput;
        RenderTexture verBlurOutput;
        RenderTexture tmpSource;

        ComputeBuffer weightsBuffer;



        public float Radius { get; private set; } = 1;
        public GaussianBlurStatic()
        {
            if (!SystemInfo.supportsComputeShaders)
            {
                throw new System.Exception("It seems your target Hardware does not support Compute Shaders.");
            }
            blurShader = Resources.Load<ComputeShader>("Shaders/Compute/GaussianBlurStatic");

            HorId = blurShader.FindKernel("HorzBlurCs");
            VerId = blurShader.FindKernel("VertBlurCs");

            blurShader.SetInt("blurRadius", (int)Radius);

        }
        ~GaussianBlurStatic()
        {
            weightsBuffer.Dispose();
        }
        public void Disptach(RenderTexture source, ref RenderTexture output)
        {
            if(source == null || output == null)
            {
                Log.Error("source or output can be null");
            }
            if(horBlurOutput != null)
            {
                if(horBlurOutput.width != source.width || horBlurOutput.height != source.height)
                {
                    horBlurOutput.Release();
                    createHorBlurOutput(source);
                }
            }
            else
            {
                createHorBlurOutput(source);
            }
            if (verBlurOutput != null)
            {
                if (verBlurOutput.width != source.width || verBlurOutput.height != source.height)
                {
                    verBlurOutput.Release();
                    createVerBlurOutput(source);
                }
            }
            else
            {
                createVerBlurOutput(source);
            }
            if(tmpSource != null)
            {
                if (verBlurOutput.width != source.width || verBlurOutput.height != source.height)
                {
                    tmpSource.Release();
                    tmpSource = new RenderTexture(source);
                    blurShader.SetTexture(HorId, "source", tmpSource);
                }
            }
            else
            {
                tmpSource = new RenderTexture(source);
                blurShader.SetTexture(HorId, "source", tmpSource);
            }


            int horizontalBlurDisX = Mathf.CeilToInt(((float)source.width / (float)1024)); // it is here becouse res of window can change
            int horizontalBlurDisY = Mathf.CeilToInt(((float)source.height / (float)1024));

            Graphics.Blit(source, tmpSource);
            blurShader.Dispatch(HorId, horizontalBlurDisX, source.height, 1);
            blurShader.Dispatch(VerId, source.width, horizontalBlurDisY, 1);

            Graphics.Blit(verBlurOutput, output);
        }
        public void SetRadius(float radius)
        {
            if(Radius != radius)
            {
                Radius = radius;
                CalculateWeights();
            }

        }
        void createHorBlurOutput(RenderTexture source)
        {
            horBlurOutput = new RenderTexture(source);
            horBlurOutput.Create();
            blurShader.SetTexture(HorId, "horBlurOutput", horBlurOutput);
            blurShader.SetTexture(VerId, "horBlurOutput", horBlurOutput);
        }
        void createVerBlurOutput(RenderTexture source)
        {
            verBlurOutput = new RenderTexture(source);
            verBlurOutput.Create();
            blurShader.SetTexture(VerId, "verBlurOutput", verBlurOutput);
        }
        float[] OneDimensinalKernel(int radius, float sigma)
        {
            float[] kernelResult = new float[radius * 2 + 1];
            float sum = 0.0f;
            for (int t = 0; t < radius; t++)
            {
                double newBlurValue = 0.39894 * Mathf.Exp(-0.5f * t * t / (sigma * sigma)) / sigma;
                kernelResult[radius + t] = (float)newBlurValue;
                kernelResult[radius - t] = (float)newBlurValue;
                if (t != 0)
                    sum += (float)newBlurValue * 2.0f;
                else
                    sum += (float)newBlurValue;
            }
            // normalize kernels
            for (int k = 0; k < radius * 2 + 1; k++)
            {
                kernelResult[k] /= sum;
            }
            return kernelResult;
        }
        private void CalculateWeights()
        {

            if (weightsBuffer != null)
                weightsBuffer.Dispose();

            float sigma = ((int)Radius) / 1.5f;

            weightsBuffer = new ComputeBuffer((int)Radius * 2 + 1, sizeof(float));
            float[] blurWeights = OneDimensinalKernel((int)Radius, sigma);
            weightsBuffer.SetData(blurWeights);

            blurShader.SetBuffer(HorId, "gWeights", weightsBuffer);
            blurShader.SetBuffer(VerId, "gWeights", weightsBuffer);
            blurShader.SetInt("blurRadius", (int)Radius);
        }
    }
}
