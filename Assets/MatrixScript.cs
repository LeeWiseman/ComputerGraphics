using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //matrixTRS();

        //trivially accept set of points

        //Vector2 start = new Vector2(0.5f, 0.5f);
        //Vector2 end = new Vector2(-0.5f, -0.5f);

        //trivially reject set of points

        //Vector2 start = new Vector2(-2, -2);
        //Vector2 end = new Vector2(-5, -5);

        //word to do set of points

        Vector2 start = new Vector2(-2, 2);
        Vector2 end = new Vector2(2, 0);

        

        if (!ClipLine(ref start, ref end))
        {
            print("Point " + start.ToString() + "," + end.ToString() + " was trivially rejected");
        }
        


    }
    // Update is called once per frame
    void Update()
    {

    }

    //matrices
    public static void matrixTRS()
    {

        //define cubes vertices
        Vector3[] cube = new Vector3[8];
        cube[0] = new Vector3(1, 1, 1);
        cube[1] = new Vector3(-1, 1, 1);
        cube[2] = new Vector3(-1, -1, 1);
        cube[3] = new Vector3(1, -1, 1);
        cube[4] = new Vector3(1, 1, -1);
        cube[5] = new Vector3(-1, 1, -1);
        cube[6] = new Vector3(-1, -1, -1);
        cube[7] = new Vector3(1, -1, -1);

        print("Cube Vertices\n");
        printMatrix(cube);

        //rotation
        Vector3 startAxis = new Vector3(-2, 1, 1);
        startAxis.Normalize();

        Quaternion rotationQ = Quaternion.AngleAxis(-25, startAxis);

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(new Vector3(0, 0, 0), rotationQ, Vector3.one);
        print("Matrix4x4 for rotation" + rotationMatrix.ToString());

        Vector3[] cubeAfterRotation = ApplyTransformation(cube, rotationMatrix);
        print("Cube After Rotation");
        printMatrix(cubeAfterRotation);

        //scale
        Matrix4x4 scaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(2, 2, 1));
        Vector3[] afterScaling = ApplyTransformation(cubeAfterRotation, scaleMatrix);
        print("Cube after Rotation and Scaling");
        printMatrix(afterScaling);

        //Translation
        Matrix4x4 translationMatrix = Matrix4x4.TRS(new Vector3(0, -3, 2), Quaternion.identity, Vector3.one);
        print("Cube after Rotation, Scaling, and Translation");

        Vector3[] afterTranslation = ApplyTransformation(afterScaling, translationMatrix);
        printMatrix(afterTranslation);


        //Master Matrix
        //multiply in reverse order*****************
        Matrix4x4 MasterMatrix = translationMatrix * scaleMatrix * rotationMatrix;
        
        Vector3[] cubeAfterAllTransformations = ApplyTransformation(cube, MasterMatrix);
        print("Single Matrix Transformations");
        printMatrix(cubeAfterAllTransformations);
        //should be equal to the afterTranslation print

        //View matrix
        Vector3 cameraPosition = new Vector3(0, 4, 51);
        Vector3 cameraUp = new Vector3(2, 1, -2);
        cameraUp.Normalize();
        Vector3 cameraLookAt = new Vector3(1, 2, 1);

        Vector3 cameraForward = (cameraLookAt - cameraPosition).normalized;
        Quaternion cameraRot = Quaternion.LookRotation(cameraForward, cameraUp);
        
        Matrix4x4 viewMatrix = Matrix4x4.TRS(-cameraPosition, cameraRot, Vector3.one);

        Vector3[] afterView = ApplyTransformation(cubeAfterAllTransformations, viewMatrix);

        print("After Viewing Matrix");
        printMatrix(afterView);

        //projection
        Matrix4x4 projectionMatrix = Matrix4x4.Perspective(90, (Screen.width / Screen.height), 1, 1000);

        Vector3[] afterProjection = ApplyTransformation(afterView, projectionMatrix);

        //final matrix of everything done
        Matrix4x4 FinalMatrix = projectionMatrix * viewMatrix * MasterMatrix;
        print("Matrix of all transformations, viewing and projection matrices");
        print(FinalMatrix.ToString());

        //Cube after all changes
        Vector3[] finalImage = ApplyTransformation(cube, FinalMatrix);

        print("Final Image of Cube" );
        printMatrix(finalImage);
    }

    public static Vector3[] ApplyTransformation(Vector3[] vertices, Matrix4x4 matrix)
    {
        Vector3[] afterTransform = new Vector3[vertices.Length];
         for(int i = 0; i < vertices.Length; i++)
        {
            afterTransform[i] = matrix * new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, 1);
            
        }
        return afterTransform;
    }

    public static void printMatrix(Vector3[] matrix)
    {
        string matrixString = "";
        for (int count = 0; count < matrix.Length; count++)
        {
            matrixString = matrix[count] + "\n";
        }
        print(matrixString);
    }

    //line clipping

    public bool ClipLine(ref Vector2 lineStart, ref Vector2 lineEnd)
    {
        Outcode startOutcode = new Outcode(lineStart);
        Outcode endOutcode = new Outcode(lineEnd);


        //Trivial Acceptance
        if (Outcode.trivialAcceptance(startOutcode,endOutcode))
        {
            print("Trivially Accepted");
            return true;
        }
        //Trivial rejection
        if (Outcode.trivialRejection(startOutcode, endOutcode))
        {
            print("Trivially Rejected");
            return false;
        }

        //work to be done

        if (startOutcode.above)
        {
            lineStart = interceptOfLine(0, lineStart, lineEnd);
            return ClipLine(ref lineStart, ref lineEnd);
        }
        if (startOutcode.below)
        {
            lineStart = interceptOfLine(1, lineStart, lineEnd);
            return ClipLine(ref lineStart, ref lineEnd);
        }

        if (startOutcode.left)
        {
            lineStart = interceptOfLine(2, lineStart, lineEnd);
            return ClipLine(ref lineStart, ref lineEnd);
        }
        if (startOutcode.below)
        {
            lineStart = interceptOfLine(3, lineStart, lineEnd);
            return ClipLine(ref lineStart, ref lineEnd);
        }

        return ClipLine(ref lineStart, ref lineEnd);

    }

    private float getSlopeOfLine(Vector2 X, Vector2 Y)
    {
        return((Y.y - X.y)/(Y.x-X.x));
    }

    private Vector2 interceptOfLine(int edge,Vector2 start, Vector2 end)
    {
        float m = getSlopeOfLine(start, end);

        switch (edge)
        {
            case 0: //Top Edge y=1
                return new Vector2(start.x + (1 / m) * (1 - start.y), 1);
            case 1: // Bottom Edge  y = -1
                return new Vector2(start.x + (1 / m) * (-1 - start.y), -1);

            case 2: // Left Edge x = -1
                return new Vector2(-1, (start.y + (m * (-1 - start.x))));

            default: // Right Edge x = 1
                return new Vector2(1, (start.y + (m * (1 - start.x))));

            
        }
    }

   

   
}
