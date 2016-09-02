using System.Collections.Generic;
using UnityEngine;

namespace toinfiniityandbeyond.Tilemapping
{
    //[CreateAssetMenu(fileName = "AnimatedTile", menuName = "Tilemap/Tiles/AnimatedTile")]
    public class AnimatedTile : ScriptableTile
    {
        [System.Serializable]
        public struct TileKeyframe {
            public float time;
            public Sprite sprite;
            public TileKeyframe(float k, Sprite v) {
                time = k;
                sprite = v;
            } 
        }
        public float playbackSpeed = 1;
        public bool globalAnimation = false;
        public List<TileKeyframe> keyframes = new List<TileKeyframe>();
        private int lastIndex = -1;

        public override bool IsValid
        {
            get
            {
                // if (dopesheet[0] == null)
                //     return false;

                // try
                // {
				// 	dopesheet[0].Value.texture.GetPixel(0, 0);
                // }
                // catch (UnityException e)
                // {
                //     return false;
                // }
                return true;
            }
        }
        public void AddKeyframe(float time, Sprite sprite) {
            for(int i = 0; i < keyframes.Count; i++) {
                if(keyframes[i].time == time) {
                    time += 0.1f;
                    break;
                }
            }
            keyframes.Add(new TileKeyframe(time, sprite));
        }
        public override Sprite GetSprite(TileMap tilemap = null, Point position = default(Point))
        {
			//Get a unique number for the 2D position
			int positionSeed = position.x >= position.y ? position.x * position.x + position.x + position.y : position.x + position.y * position.y;
           	System.Random prng = new System.Random(positionSeed);
			//Set the index = to the current time * animation speed
            int index = (int)(Time.time * playbackSpeed);
			//If it is a global animation (ie all instances play at the same time) then skip this
            //if (!globalAnimation)
            //   index += prng.Next(0, keyframes.Count);
			//Modolo to get the remainder. so 0 <= index < amount of keyframes 
            index %= keyframes.Count;
            if(index != lastIndex)
                lastIndex = index;
            return keyframes[lastIndex].sprite;
        }
        public override Texture2D GetIcon()
        {
           if (!IsValid) return null;
			return keyframes[0].sprite.ToTexture2D();
        }
      
        public override float TickRate { get { return 0.1f; } }
        protected override bool Tick()
        {
            return true;
        }
    }
}
