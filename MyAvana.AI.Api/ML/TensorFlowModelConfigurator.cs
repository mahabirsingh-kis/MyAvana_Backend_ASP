using Microsoft.ML;
using MyAvanaApi.ML.DataModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.ML
{
    public class TensorFlowModelConfigurator
    {
        public static  MLContext _mlContext;
        public static  ITransformer _mlModel;

        static readonly string _assetsPath = Path.Combine(Environment.CurrentDirectory, "assets");
        static ITransformer mlModel;



        static readonly string _trainTagsTsv = Path.Combine(_assetsPath, "inputs-train", "file.tsv");
        static readonly string _predictImageListTsv = Path.Combine(_assetsPath, "inputs-predict", "data", "image_list.tsv");
        static readonly string _trainImagesFolder = Path.Combine(_assetsPath, "inputs-train");
        static readonly string _predictImagesFolder = Path.Combine(_assetsPath, "inputs-predict", "data");
        static readonly string _predictSingleImage = Path.Combine(_assetsPath, "inputs-predict-single", "54250118kinkytwist.jpg");
        static readonly string _inceptionPb = Path.Combine(_assetsPath, "inception", "tensorflow_inception_graph.pb");
        static readonly string _inputImageClassifierZip = Path.Combine(_assetsPath, "inputs-predict", "imageClassifier.zip");
        static readonly string _outputImageClassifierZip = Path.Combine(_assetsPath, "outputs", "imageClassifier.zip");


        private static string LabelTokey = nameof(LabelTokey);
        private static string PredictedLabelValue = nameof(PredictedLabelValue);



        public static void Initialize()
        {
            _mlContext = new MLContext();

            // Model creation and pipeline definition for images needs to run just once, so calling it from the constructor:
            _mlModel = SetupMlnetModel();
        }

       
        private struct InceptionSettings
        {
            public const int ImageHeight = 224;
            public const int ImageWidth = 224;
            public const float Mean = 117;
            public const float Scale = 1;
            public const bool ChannelsLast = true;
        }

        public struct ImageSettings
        {
            public const int imageHeight = 227;
            public const int imageWidth = 227;
            public const float mean = 117;         //offsetImage
            public const bool channelsLast = true; //interleavePixelColors
        }

        public struct TensorFlowModelSettings
        {
            // input tensor name
            public const string inputTensorName = "Placeholder";

            // output tensor name
            public const string outputTensorName = "loss";
        }


        // For checking tensor names, you can open the TF model .pb file with tools like Netron: https://github.com/lutzroeder/netron

        private static ITransformer SetupMlnetModel()
        {
            #region OldCode
            var data = _mlContext.Data.LoadFromTextFile<ImageData>(_trainTagsTsv, hasHeader: false);

            var estimator = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: LabelTokey, inputColumnName: "Label")
                            .Append(_mlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: _trainImagesFolder, inputColumnName: nameof(ImageData.ImagePath)))
                            .Append(_mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputColumnName: "input"))
                            .Append(_mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean))
                            .Append(_mlContext.Model.LoadTensorFlowModel(_inceptionPb).
                                 ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true))
                            .Append(_mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: LabelTokey, featureColumnName: "softmax2_pre_activation"))
                            .Append(_mlContext.Transforms.Conversion.MapKeyToValue(PredictedLabelValue, "PredictedLabel"))
                            .AppendCacheCheckpoint(_mlContext);


            mlModel = estimator.Fit(data);

            var predictions = mlModel.Transform(data);


            var multiclassContext = _mlContext.MulticlassClassification;
            var metrics = multiclassContext.Evaluate(predictions, labelColumnName: LabelTokey, predictedLabelColumnName: "PredictedLabel");
            #endregion



            return mlModel;
        }
      
        public static (string LabelValue, Dictionary<String, float> score, string label) PredictImage(string ImagePath)
        {
            var imageData = new ImageData()
            {
                ImagePath = ImagePath
            };
            // </SnippetLoadImageData>  

            // <SnippetPredictSingle>  
            // Make prediction function (input = ImageData, output = ImagePrediction)
            var predictor = _mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(mlModel);
            ImagePrediction imagePrediction = new ImagePrediction();
            var prediction = predictor.Predict(imageData);
            // </SnippetPredictSingle> 

            Dictionary<String, float> keyValuePairs = new Dictionary<string, float>();
            for (int i = 0; i < prediction.Score.Length; i++)
            {
                if(i==0)
                    keyValuePairs.Add("Straight", prediction.Score[i] * 100);
                else if(i==1)
                    keyValuePairs.Add("Wavy", prediction.Score[i] * 100);
                else if (i == 2)
                    keyValuePairs.Add("Curly", prediction.Score[i] * 100);
                else if (i == 3)
                    keyValuePairs.Add("Kinky", prediction.Score[i] * 100);
                else if (i == 4)
                    keyValuePairs.Add("StraightBraids", prediction.Score[i] * 100);
                else if (i == 5)
                    keyValuePairs.Add("Locks", prediction.Score[i] * 100);
                else if (i == 6)
                    keyValuePairs.Add("Bald", prediction.Score[i] * 100);
            }
            //return (prediction.PredictedLabelValue, prediction.Score.Max(), GetLabelValue(prediction.PredictedLabelValue));
            return (prediction.PredictedLabelValue, keyValuePairs, GetLabelValue(prediction.PredictedLabelValue));
        }

        public void SaveMLNetModel(string mlnetModelFilePath)
        {
            // Save/persist the model to a .ZIP file to be loaded by the PredictionEnginePool
            _mlContext.Model.Save(_mlModel, null, mlnetModelFilePath);
        }
        private static string GetLabelValue(string label)
        {
            switch (label.ToLower())
            {
                case "kinky":
                    return "“kinky” or more appropriately full of tight coils (tightly curled hair). Typically, Type 4 hair is also extremely wiry and fragile.Often times, it appears to be coarse, however, it is really very fine, with several thin hair strands densely packed together.";
                case "straight":
                    return "As a whole, straight hair tends to be shiny. That is because the straight follicles make it easier for natural oils to travel up and down your strands.When your hair is wet, add a quarter - sized amount of mousse to the roots and give it a little massage. Blow dry your hair upside down, or with a big round barrel brush, and style it in the opposite direction to give it some volume on the top.When it’s dry, spritz on some hair spray and back comb the areas that need a little more help.";
                case "wavy":
                    return "Your waves are fine and thin with a loose, tousled texture. Wavy texture is not quite straight and not completely curly, with the spectrum of hair ranging from loose loops to coarse, thick S-shaped waves combined with curls. Type 2 texture is typically flatter at the root and lays close to the head, getting curlier from the ears down.Your lack of volume and definition means that products can easily weigh them down and strands can become straight so use lighter styling products like mousses and gels.";
                case "coily":
                    return "Your dense, springy coils are either wiry or fine, and have the circumference of a crochet needle. They are tightly coiled, with a visible S pattern. Use thicker natural emollients like mango and shea butters to maximize your wash - and - go, twist -out, or bantu knot -out style after washing.";
                case "curly":
                    return "You have big, loose curls and spirals similar in circumference to a piece of thick, sidewalk chalk. Your curls tend to be shiny, with a well-defined S-shape. Use an anti - humectant(humidity blocking) styling cream or styling milk for less frizz but more definition.";
                default:
                    return "";
            }
        }
    }
}
