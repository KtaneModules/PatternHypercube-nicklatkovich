﻿using System.Linq;
using UnityEngine;

namespace _4D {
	public class Matrix5x5 {
		public static readonly Matrix5x5 IDENTITY = new Matrix5x5();

		public static Matrix5x5 operator *(Matrix5x5Int m5a, Matrix5x5 m5b) { return new Matrix5x5(m5a) * m5b; }
		public static Matrix5x5 operator *(Matrix5x5 m5a, Matrix5x5Int m5b) { return m5a * new Matrix5x5(m5b); }
		public static Matrix5x5 operator *(Matrix5x5 m5a, Matrix5x5 m5b) {
			Matrix5x5 result = new Matrix5x5();
			for (int row = 0; row < 5; row++) {
				for (int column = 0; column < 5; column++) result._data[row][column] = Enumerable.Range(0, 5).Select(i => m5a._data[row][i] * m5b._data[i][column]).Sum();
			}
			return result;
		}

		public static Vector3 operator *(Vector3 v3, Matrix5x5 m5) {
			Vector4 v4 = Utils.ToVector4(v3, 1) * m5;
			// return Utils.Flat(v4);
			return Utils.Div(Utils.Flat(v4), v4.w);
		}

		public static Vector4 operator *(Vector4 v4, Matrix5x5 m5) {
			Vector5 v5 = Utils.ToVector5(v4, 1) * m5;
			// return v5.flat;
			return Utils.Div(v5.flat, v5.v);
		}

		public static Vector5 operator *(Vector5 v5, Matrix5x5 m5) {
			float[] v5c = v5.GetComponents();
			return Vector5.FromComponents(Enumerable.Range(0, 5).Select(i => Enumerable.Range(0, 5).Select(j => v5c[j] * m5._data[j][i]).Sum()).ToArray());
		}

		public static Matrix5x5 Rotation(int componentAIndex, int componentBIndex, float rads) {
			if (componentAIndex < 0 || componentBIndex < 0 || componentAIndex > 3 || componentBIndex > 3 || componentAIndex == componentBIndex) throw new System.Exception();
			Matrix5x5 result = new Matrix5x5();
			float cos = Mathf.Cos(rads);
			float sin = Mathf.Sin(rads);
			result._data[componentAIndex][componentAIndex] = cos;
			result._data[componentAIndex][componentBIndex] = sin;
			result._data[componentBIndex][componentAIndex] = -sin;
			result._data[componentBIndex][componentBIndex] = cos;
			return result;
		}

		public static Matrix5x5 Translation(Vector4 v4) {
			Matrix5x5 result = new Matrix5x5();
			result._data[4][0] = v4.x;
			result._data[4][1] = v4.y;
			result._data[4][2] = v4.z;
			result._data[4][3] = v4.w;
			return result;
		}
	
		public static Matrix5x5 Perpective(float fov, float near, float far) {
			float s = 1f / (Mathf.Tan(fov / 2 * Mathf.PI / 180));
			float diff = far - near;
			Matrix5x5 res = new Matrix5x5();
			res._data[0][0] = s;
			res._data[1][1] = s;
			res._data[2][2] = s;
			res._data[3][3] = -far / diff;
			res._data[3][4] = -1;
			res._data[4][3] = -far * near / diff;
			res._data[4][4] = 0;
			return res;
		}

		public float this[int row, int column] { get { return _data[row][column]; } }
		private float[][] _data;
		public Matrix5x5() { _data = Enumerable.Range(0, 5).Select(r => Enumerable.Range(0, 5).Select(c => r == c ? 1f : 0f).ToArray()).ToArray(); }
		public Matrix5x5(Matrix5x5Int m5i) { _data = Enumerable.Range(0, 5).Select(r => Enumerable.Range(0, 5).Select(c => (float)m5i[r, c]).ToArray()).ToArray(); }
	}
}
