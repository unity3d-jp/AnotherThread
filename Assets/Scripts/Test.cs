using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UTJ {

public class Test : MonoBehaviour {

	// Vector3 vector_;

	// void setVector(Vector3 v)
	// {
	// 	vector_ += v;
	// }

	// void setVector2(Vector3 v)
	// {
	// 	vector_.x += v.x;
	// 	vector_.y += v.y;
	// 	vector_.z += v.z;
	// }

	// void setVector(ref Vector3 v)
	// {
	// 	vector_ += v;
	// }

	// void setVector2(ref Vector3 v)
	// {
	// 	vector_.x += v.x;
	// 	vector_.y += v.y;
	// 	vector_.z += v.z;
	// }

	// void setVector(float x, float y, float z)
	// {
	// 	vector_.x += x;
	// 	vector_.y += y;
	// 	vector_.z += z;
	// }

	// IEnumerator loop()
	// {
	// 	yield return null;
	// 	Debug.Log("test start");

	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	//     {
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			setVector(Vector3.zero);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 520
	// 	}
	//     {
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			setVector2(Vector3.zero);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 341
	// 	}
	//     {
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		var vect = Vector3.one;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			setVector(ref vect);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 348
	// 	}
	//     {
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		var vect = Vector3.one;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			setVector2(ref vect);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 174 This is the best!
	// 	}
	//     {
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		var vect = Vector3.one;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			setVector(vect.x, vect.y, vect.z);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 191
	// 	}

	// 	yield return null;
	// 	Debug.Log("test end");
	// }

	// class A {
	// 	public int member;
	// 	public A(int i) { member = i; }
	// }

	// IEnumerator loop()
	// {
	// 	var list0 = new List<A>(32);
	// 	for (var i = 0; i < 32; ++i) {
	// 		list0.Add(new A(i));
	// 	}
	// 	Debug.Log(list0.Count);

	// 	var list1 = new LinkedList<A>();
	// 	list1.AddFirst(list0[4]);
	// 	list1.AddFirst(list0[3]);
	// 	list1.AddFirst(list0[2]);
	// 	list1.AddFirst(list0[1]);
	// 	Debug.Log(list1.Count);
	// 	Debug.Log(list1.First.Value.member);

	// 	yield return null;
		
	// }


	// Vector3 vector_;

	// void setVector0(Vector3 v)
	// {
	// 	vector_ = v;
	// }
	// void setVector1(ref Vector3 v)
	// {
	// 	vector_ = v;
	// }
	// void setVector2(ref Vector3 v)
	// {
	// 	vector_.x = v.x;
	// 	vector_.y = v.y;
	// 	vector_.z = v.z;
	// }

	// IEnumerator loop()
	// {
	// 	yield return null;
	// 	Debug.Log("test start");

	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	//     {
	// 		long begin_time;
	// 		begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			setVector0(Vector3.zero);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 608

	// 		begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			var v = Vector3.zero;
	// 			setVector0(v);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 646

	// 		begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			var v = new Vector3(0f, 0f, 0f);
	// 			setVector0(v);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 478

	// 		begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			var v = Vector3.zero;
	// 			setVector1(ref v);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 646

	// 		begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			var v = Vector3.zero;
	// 			setVector2(ref v);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 673

	// 		var v0 = Vector3.zero;
	// 		begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			setVector1(ref v0);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 242

	// 		var v1 = Vector3.zero;
	// 		begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 			setVector2(ref v1);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 265
	// 	}

	// 	yield return null;
	// 	Debug.Log("test end");
	// }

	// IEnumerator loop()
	// {
	// 	yield return null;
	// 	Debug.Log("test start");
	// 	byte[] buf;

	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	//     {
	// 		long begin_time;
	// 		for (var i = 0; i < 256; ++i) {
	// 			for (var j = 0; j < 65536; ++j) {
	// 				buf = new byte[1024];
	// 			}
	// 			yield return null;
	// 		}
	// 		buf = null;
	// 		yield return new WaitForSeconds(2);
	// 		begin_time = sw.ElapsedMilliseconds;
	// 		System.GC.Collect();
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time));
	// 		yield return null;

	// 		begin_time = sw.ElapsedMilliseconds;
	// 		System.GC.Collect();
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time));
	// 	}

	// 	yield return null;
	// 	Debug.Log("test end");
	// }

	// struct MyMatrix
	// {
	// 	float m00, m01, m02, m03;
	// 	float m10, m11, m12, m13;
	// 	float m20, m21, m22, m23;
	// 	float m30, m31, m32, m33;

	// 	public static MyMatrix identity { get {
	// 			var m = new MyMatrix();
	// 			m.m00 = 1;
	// 			m.m01 = 0;
	// 			m.m02 = 0;
	// 			m.m03 = 0;
	// 			m.m10 = 0;
	// 			m.m11 = 1;
	// 			m.m12 = 0;
	// 			m.m13 = 0;
	// 			m.m20 = 0;
	// 			m.m21 = 0;
	// 			m.m22 = 1;
	// 			m.m23 = 0;
	// 			m.m30 = 0;
	// 			m.m31 = 0;
	// 			m.m32 = 0;
	// 			m.m33 = 1;
	// 			return m;
	// 		}
	// 	}

	// 	public static MyMatrix mul(ref MyMatrix a, ref MyMatrix b)
	// 	{
	// 		var m = new MyMatrix();
	// 		m.m00 = a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30;
	// 		m.m01 = a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31;
	// 		m.m02 = a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32;
	// 		m.m03 = a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33;
	// 		m.m10 = a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30;
	// 		m.m11 = a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31;
	// 		m.m12 = a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32;
	// 		m.m13 = a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33;
	// 		m.m20 = a.m20 * b.m00 + a.m21 * b.m20 + a.m22 * b.m20 + a.m23 * b.m30;
	// 		m.m21 = a.m20 * b.m01 + a.m21 * b.m21 + a.m22 * b.m21 + a.m23 * b.m31;
	// 		m.m22 = a.m20 * b.m02 + a.m21 * b.m22 + a.m22 * b.m22 + a.m23 * b.m32;
	// 		m.m23 = a.m20 * b.m03 + a.m21 * b.m23 + a.m22 * b.m23 + a.m23 * b.m33;
	// 		m.m30 = a.m30 * b.m00 + a.m31 * b.m30 + a.m32 * b.m30 + a.m33 * b.m30;
	// 		m.m31 = a.m30 * b.m01 + a.m31 * b.m31 + a.m32 * b.m31 + a.m33 * b.m31;
	// 		m.m32 = a.m30 * b.m02 + a.m31 * b.m32 + a.m32 * b.m32 + a.m33 * b.m32;
	// 		m.m33 = a.m30 * b.m03 + a.m31 * b.m33 + a.m32 * b.m33 + a.m33 * b.m33;
	// 		return m;
	// 	}
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	// 	{
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		var m = Matrix4x4.identity;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			m = m * m;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 537
	// 	}
	// 	{
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		var m = MyMatrix.identity;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			m = MyMatrix.mul(ref m, ref m);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 389  ref を付けなければほぼ互角
	// 	}
	// 	yield return null;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	var rigidbodies = new UTJ.RigidbodyTransform[100];
	// 	sw.Start();
	// 	{
	// 		float a = 0;
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 100000000; ++i) {
	// 			a += rigidbodies[10].transform_.position_.x;
	// 			a += rigidbodies[10].transform_.position_.y;
	// 			a += rigidbodies[10].transform_.position_.z;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 1303
	// 	}
	// 	{
	// 		float a = 0;
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 100000000; ++i) {
	// 			var position = rigidbodies[10].transform_.position_;
	// 			a += position.x;
	// 			a += position.y;
	// 			a += position.z;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 1406 微差。
	// 	}
	// 	yield return null;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	var rigidbodies = new UTJ.RigidbodyTransform[100];
	// 	var positions = new Vector3[100];
	// 	sw.Start();
	// 	{
	// 		float a = 0;
	// 		long begin_time = sw.ElapsedTicks;
	// 		// long begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 100000000; ++i) {
	// 			a += rigidbodies[i&0xf].transform_.position_.x;
	// 		}
	// 		// Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 556
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 556
	// 	}
	// 	{
	// 		float a = 0;
	// 		long begin_time = sw.ElapsedMilliseconds;
	// 		for (var i = 0; i < 100000000; ++i) {
	// 			a += positions[i%0xf].x;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedMilliseconds - begin_time)); // 788 why?
	// 	}
	// 	yield return null;
	// }

	// public Matrix4x4 mat0_;

	// private Matrix4x4 transpose(ref Matrix4x4 mat)
	// {
	// 	Matrix4x4 result;
	// 	result.m00 = mat.m00;
	// 	result.m01 = mat.m10;
	// 	result.m02 = mat.m20;
	// 	result.m03 = mat.m30;
	// 	result.m10 = mat.m01;
	// 	result.m11 = mat.m11;
	// 	result.m12 = mat.m21;
	// 	result.m13 = mat.m31;
	// 	result.m20 = mat.m02;
	// 	result.m21 = mat.m12;
	// 	result.m22 = mat.m22;
	// 	result.m23 = mat.m32;
	// 	result.m30 = mat.m03;
	// 	result.m31 = mat.m13;
	// 	result.m32 = mat.m23;
	// 	result.m33 = mat.m33;
	// 	return result;
	// }

	// private Matrix4x4 transpose2(ref Matrix4x4 mat)
	// {
	// 	Matrix4x4 result = mat;
	// 	result.m01 = mat.m10;
	// 	result.m02 = mat.m20;
	// 	result.m03 = mat.m30;
	// 	result.m10 = mat.m01;
	// 	result.m12 = mat.m21;
	// 	result.m13 = mat.m31;
	// 	result.m20 = mat.m02;
	// 	result.m21 = mat.m12;
	// 	result.m23 = mat.m32;
	// 	result.m30 = mat.m03;
	// 	result.m31 = mat.m13;
	// 	result.m32 = mat.m23;
	// 	return result;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	// 	{
	// 		var mat = Matrix4x4.identity;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			mat0_ = Matrix4x4.Transpose(mat);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 3544
	// 	}
	// 	{
	// 		var mat = Matrix4x4.identity;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			mat0_ = transpose(ref mat);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 2994  ref を付けなければ互角
	// 	}
	// 	{
	// 		var mat = Matrix4x4.identity;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			mat0_ = transpose2(ref mat);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 3523
	// 	}
	// 	yield return null;
	// }


	// private Vector3 vec_;
	// private void set(ref Vector3 v)
	// {
	// 	vec_ = v;
	// }
	// private void set(float x, float y, float z)
	// {
	// 	vec_.x = x;
	// 	vec_.y = y;
	// 	vec_.z = z;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		var v = new Vector3(1f, 2f, 3f);
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			set(ref v);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 1051
	// 	}
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			var v = new Vector3(1f, 2f, 3f);
	// 			set(ref v);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 2477
	// 	}
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			set(1f, 2f, 3f);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 1695
	// 	}
	// 	yield return null;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 10000000; i++) {
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 3362
	// 	}
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 10000000; ++i) {
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time)); // 2574
	// 	}
	// 	yield return null;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	// 	long overhead;
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 		}
	// 		overhead = sw.ElapsedTicks - begin_time;
	// 		Debug.Log("overhead:"+ (overhead)); // 33050
	// 	}
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			float f = Mathf.Sqrt(100f);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 89370
	// 	}
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			float f = (float)System.Math.Sqrt((double)100f);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 14760
	// 	}
	// 	yield return null;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	// 	long overhead;
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 		}
	// 		overhead = sw.ElapsedTicks - begin_time;
	// 		Debug.Log("overhead:"+ (overhead)); // 33050
	// 	}
	// 	{
	// 		Vector3 v = Vector3.zero;
	// 		Vector3 v1 = Vector3.one;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			v += v1;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 248680
	// 	}
	// 	{
	// 		Vector3 v = Vector3.zero;
	// 		Vector3 v1 = Vector3.one;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			v.x += v1.x;
	// 			v.y += v1.y;
	// 			v.z += v1.z;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 72240
	// 	}
	// 	yield return null;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	sw.Start();
	// 	long overhead;
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 		}
	// 		overhead = sw.ElapsedTicks - begin_time;
	// 		Debug.Log("overhead:"+ (overhead)); // 25060
	// 	}
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			var v = Vector3.zero;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 206550
	// 	}
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < 1000000; ++i) {
	// 			var v = new Vector3(0f, 0f, 0f);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 132580
	// 	}
	// 	yield return null;
	// }

	// float func(float v)
	// {
	// 	return v;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	const int num = 1000000;
	// 	sw.Start();
	// 	long overhead;
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 		}
	// 		overhead = sw.ElapsedTicks - begin_time;
	// 		Debug.Log("overhead:"+ (overhead)); // 30060
	// 	}
	// 	{
	// 		float a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = func(10f);
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 71310
	// 	}
	// 	{
	// 		float a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = 10f;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 16090
	// 	}
	// 	yield return null;
	// }

	// float prop { get { return 10f; } }
	// public float mbvalue = 10f;
	// private float mvvalue = 10f;
	// const float cvalue = 10f;

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	const int num = 1000000;
	// 	sw.Start();
	// 	long overhead;
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 		}
	// 		overhead = sw.ElapsedTicks - begin_time;
	// 		Debug.Log("overhead:"+ (overhead)); // 30150
	// 	}
	// 	{
	// 		float a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = prop;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 63720
	// 	}
	// 	{
	// 		float a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = mbvalue;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 14890
	// 	}
	// 	{
	// 		float a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = mvvalue;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 18590
	// 	}
	// 	{
	// 		float a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = cvalue;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 12700
	// 	}
	// 	{
	// 		float a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = 10f;
	// 		}
	// 		Debug.Log("elapsed:"+ (sw.ElapsedTicks - begin_time - overhead)); // 19960
	// 	}
	// 	yield return null;
	// }

	// static Vector3 prop { get { return new Vector3(0f, 0f, 0f); } }
	// static Vector3 vvalue = new Vector3(0f, 0f, 0f);

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	const int num = 1000000;
	// 	sw.Start();
	// 	long overhead;
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 		}
	// 		overhead = sw.ElapsedTicks - begin_time;
	// 		Debug.Log("overhead:"+ (overhead)); // 26440
	// 	}
	// 	{
	// 		Vector3 a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = prop;
	// 		}
	// 		Debug.Log("property:"+ (sw.ElapsedTicks - begin_time - overhead)); // 188290
	// 	}
	// 	{
	// 		Vector3 a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = new Vector3(0f, 0f, 0f);
	// 		}
	// 		Debug.Log("direct:"+ (sw.ElapsedTicks - begin_time - overhead)); // 183240
	// 	}
	// 	{
	// 		Vector3 a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = vvalue;
	// 		}
	// 		Debug.Log("member:"+ (sw.ElapsedTicks - begin_time - overhead)); // 23220
	// 	}
	// 	{
	// 		Vector3 a;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			a = CV.Vector3Zero;
	// 		}
	// 		Debug.Log("cv:"+ (sw.ElapsedTicks - begin_time - overhead)); // 26130
	// 	}
	// 	yield return null;
	// }

	// class Hoge
	// {
	// 	public float x, y, z;
	// 	public Hoge() { x = 0; y = 0; z = 0; }
	// }
	// void func0(Hoge a)
	// {
	// 	a.x += 0.01f;
	// }
	// void func1(ref Hoge a)
	// {
	// 	a.x += 0.01f;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	const int num = 1000000;
	// 	sw.Start();
	// 	long overhead;
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 		}
	// 		overhead = sw.ElapsedTicks - begin_time;
	// 		Debug.Log("overhead:"+ (overhead)); // 26440
	// 	}
	// 	{
	// 		Hoge a = new Hoge();
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			func0(a);
	// 		}
	// 		Debug.Log("no ref:"+ (sw.ElapsedTicks - begin_time - overhead));
	// 		Debug.Log("a.x="+a.x);
	// 	}
	// 	{
	// 		Hoge a = new Hoge();
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			func1(ref a);
	// 		}
	// 		Debug.Log("ref:"+ (sw.ElapsedTicks - begin_time - overhead));
	// 		Debug.Log("a.x="+a.x);
	// 	}
	// 	yield return null;
	// }

	// Vector3 vector_;

	// void setVector0(Vector3 v)
	// {
	// 	vector_ = v;
	// }
	// void setVector1(ref Vector3 v)
	// {
	// 	vector_ = v;
	// }
	// void setVector2(ref Vector3 v)
	// {
	// 	vector_.x = v.x;
	// 	vector_.y = v.y;
	// 	vector_.z = v.z;
	// }

	// IEnumerator loop()
	// {
	// 	var sw = new System.Diagnostics.Stopwatch();
	// 	const int num = 10000000;
	// 	sw.Start();
	// 	long overhead;
	// 	{
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 		}
	// 		overhead = sw.ElapsedTicks - begin_time;
	// 		Debug.Log("overhead:"+ (overhead)); // 26440
	// 	}
	// 	{
	// 		var v = Vector3.one;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			setVector1(ref v);
	// 		}
	// 		Debug.Log("a:"+ (sw.ElapsedTicks - begin_time - overhead));
	// 	}
	// 	{
	// 		var v = Vector3.one;
	// 		long begin_time = sw.ElapsedTicks;
	// 		for (var i = 0; i < num; ++i) {
	// 			setVector2(ref v);
	// 		}
	// 		Debug.Log("b:"+ (sw.ElapsedTicks - begin_time - overhead));
	// 	}
	// 	yield return null;
	// }

	IEnumerator loop()
	{
		var sw = new System.Diagnostics.Stopwatch();
		const int num = 100000000;
		sw.Start();
		{
			long begin_time = sw.ElapsedTicks;
			for (var i = 0; i < num; i++) {
			}
			Debug.Log("i++:"+ (sw.ElapsedTicks - begin_time));
		}
		{
			long begin_time = sw.ElapsedTicks;
			for (var i = 0; i < num; ++i) {
			}
			Debug.Log("++i:"+ (sw.ElapsedTicks - begin_time));
		}
		yield return null;
	}

	void Start () {
		StartCoroutine(loop());
	}
}

} // namespace UTJ {
