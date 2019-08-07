using System;
using System.Collections.Generic;

using Configuration;

namespace Resource
{
    public class LevelResource
    {
        // TODO 1
        public List<string> LevelFilePathList = new List<string>();
        
        private static LevelResource instance = null;
        public static LevelResource Instance
        {
          get 
          {
              if (null == instance)
              {
                  instance = new LevelResource();

              }
              return instance; 
          }
        }

        private LevelResource()
        {
            // 1. add txt in Levels
            // 2. change its property from Compile to None
            // 3. add txt name below
            LevelFilePathList.Add(Constant.LevelPath + "0.txt");
            LevelFilePathList.Add(Constant.LevelPath + "1.txt");
            LevelFilePathList.Add(Constant.LevelPath + "2.txt");
            LevelFilePathList.Add(Constant.LevelPath + "3.txt");
        }
    }


    public class LevelContent
    {
        public int ColumnNum
        {
            get
            {
                if (0 == LineList.Count)
                {
                    return 0;
                }
                return LineList[0].Length;
            }
        }

        public int LineNum
        {
            get
            {
                return LineList.Count;
            }
        }

        public List<string> LineList = new List<string>();

        public void Check()
        {
            string line0 = LineList[0];
            foreach (string line in LineList)
            {
                if (line.Length != line0.Length)
                {
                    throw new Exception(String.Format("Content lines have different length."));
                }
            }
        }
    }

}
