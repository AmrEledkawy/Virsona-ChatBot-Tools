/******************************************************************\
 *      Class Name:     RedBlackNode
 *      Written By:     James.R
 *      Copyright:      Virsona Inc
 *
 *      Modifications:
 *      -----------------------------------------------------------
 *      Date            Author          Modification
 *
\******************************************************************/
///<summary>
/// The RedBlackNode class encapsulates a node in the tree
///</summary>

using System;
using System.Text;
using DataTemple;
using PluggerBase.FastSerializer;
using GenericTools;

namespace DataTemple.Codeland.SearchTree
{

    public class RedBlackNode<KeyType, DataType> : IFastSerializable, IReusable
        where KeyType : IComparable<KeyType>
    {
		// key provided by the calling class
		private KeyType ordKey;
		// the data or value associated with the key
		private DataType objData;
		// color - used to balance the tree
		private int intColor;
		// left node 
        private RedBlackNode<KeyType, DataType> rbnLeft;
		// right node 
        private RedBlackNode<KeyType, DataType> rbnRight;
        // parent node 
        private RedBlackNode<KeyType, DataType> rbnParent;

        public RedBlackNode()
        {
            Color = 0;   // red
        }

        // Initialize a sentinel node (use BLACK as color)
        public RedBlackNode(int color)
        {
            rbnLeft = rbnRight = rbnParent = null;
            intColor = color;
        }

        ///<summary>
		///Key
		///</summary>
		public KeyType Key
		{
			get
            {
				return ordKey;
			}
			
			set
			{
				ordKey = value;
			}
		}
		///<summary>
		///Data
		///</summary>
		public DataType Data
		{
			get
            {
				return objData;
			}
			
			set
			{
				objData = value;
			}
		}
		///<summary>
		///Color
		///</summary>
		public int Color
		{
			get
            {
				return intColor;
			}
			
			set
			{
				intColor = value;
			}
		}
		///<summary>
		///Left
		///</summary>
        public RedBlackNode<KeyType, DataType> Left
		{
			get
            {
				return rbnLeft;
			}
			
			set
			{
				rbnLeft = value;
			}
		}
		///<summary>
		/// Right
		///</summary>
        public RedBlackNode<KeyType, DataType> Right
		{
			get
            {
				return rbnRight;
			}
			
			set
			{
				rbnRight = value;
			}
		}
        public RedBlackNode<KeyType, DataType> Parent
        {
            get
            {
                return rbnParent;
            }
			
            set
            {
                rbnParent = value;
            }
        }

        public override string ToString()
        {
            if (objData == null) // looks like the sentinal node
                return "*";
            else
                return ordKey.ToString() + ":" + objData.ToString() + "(" + this.GetHashCode() + ")";
        }

        #region IFastSerializable Members

        public void Deserialize(SerializationReader reader)
        {
            ordKey = (KeyType) reader.ReadObject();   // ordKey = (KeyType)info.GetValue("ordKey", typeof(KeyType));
            objData = (DataType) reader.ReadPointer(); // objData = (DataType)info.GetValue("objData", typeof(DataType));
            intColor = reader.ReadInt32();  // intColor = info.GetInt32("intColor");
            rbnLeft = (RedBlackNode<KeyType, DataType>)reader.ReadPointer(); // rbnLeft = (RedBlackNode<KeyType, DataType>)info.GetValue("rbnLeft", typeof(RedBlackNode<KeyType, DataType>));
            rbnRight = (RedBlackNode<KeyType, DataType>)reader.ReadPointer();    // rbnRight = (RedBlackNode<KeyType, DataType>)info.GetValue("rbnRight", typeof(RedBlackNode<KeyType, DataType>));
            rbnParent = (RedBlackNode<KeyType, DataType>)reader.ReadPointer();   // rbnParent = (RedBlackNode<KeyType, DataType>)info.GetValue("rbnParent", typeof(RedBlackNode<KeyType, DataType>));
        }

        public void Serialize(SerializationWriter writer)
        {
            writer.WriteObject(ordKey);   // info.AddValue("ordKey", ordKey);
            writer.WritePointer(objData);   // info.AddValue("objData", objData);
            writer.Write(intColor); // info.AddValue("intColor", intColor);
            writer.WritePointer(rbnLeft);   // info.AddValue("rbnLeft", rbnLeft);
            writer.WritePointer(rbnRight);  // info.AddValue("rbnRight", rbnRight);
            writer.WritePointer(rbnParent); // info.AddValue("rbnParent", rbnParent);
        }

        #endregion

        #region IReusable Members

        public void Reset()
        {
            Color = 0;   // red
            rbnLeft = rbnRight = rbnParent = null;
            // don't reset key and data-- new allocater should!
        }

        #endregion
    }
}
