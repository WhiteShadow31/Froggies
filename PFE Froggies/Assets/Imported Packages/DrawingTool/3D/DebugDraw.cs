using System.Collections.Generic;
using UnityEngine;

namespace DrawingTool
{
   public enum AXIS
   {
            X, Y, Z
   }

    public class DebugDraw
    {
        // ==================== ----- DRAWING
        ///
        // ===== ----- CIRCLE

        /// <summary>
        /// Draw a circle around the position based on the spawnRadius, rotation inputs.
        /// </summary>
        /// <param name="position"> Position of the center of the circle </param>
        /// <param name="radius"> Radius of the circle </param>
        /// <param name="color"> Color of the circle </param>
        /// <param name="axis"> Drawing axis </param>
        /// <param name="rotation"> Rotation of the circle </param>
        /// <param name="scale"> Scale of the circle </param>
        /// <param name="nbrOfPoints"> The number of points composing the circle </param>
        public static void Circle(Vector3 position, float radius, Color color, DrawingTool.AXIS axis, Quaternion rotation, Vector3 scale, int nbrOfPoints = 45)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = secureColor.a > 0 ? secureColor.a : 1;

            Gizmos.color = secureColor;

            // If either spawnRadius or number of segments are less or equal to 0, skip drawing
            if (radius <= 0.0f || nbrOfPoints <= 0)
            {
                return;
            }

            // Single segment of the circle covers (360 / number of segments) degrees
            float angleStep = (360.0f / nbrOfPoints);

            // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
            // which are required by Unity's Mathf class trigonometry methods

            angleStep *= Mathf.Deg2Rad;

            // lineStart and lineEnd variables are declared outside of the following for loop
            Vector3 lineStart = Vector3.zero;
            Vector3 lineEnd = Vector3.zero;

            for (int i = 0; i < nbrOfPoints; i++)
            {
                Vector3 startValues = Vector3.zero;
                Vector3 endValues = Vector3.zero;


                switch (axis)
                {
                    case DrawingTool.AXIS.Z:
                        // Path start is defined as starting angle of the current segment (i)
                        startValues.x = Mathf.Cos(angleStep * i);
                        startValues.y = Mathf.Sin(angleStep * i);
                        startValues.z = 0.0f;

                        // Path end is defined by the angle of the next segment (i+1)
                        endValues.x = Mathf.Cos(angleStep * (i + 1));
                        endValues.y = Mathf.Sin(angleStep * (i + 1));
                        endValues.z = 0.0f;
                        break;
                    case DrawingTool.AXIS.Y:
                        startValues.x = Mathf.Cos(angleStep * i);
                        startValues.y = 0.0f;
                        startValues.z = Mathf.Sin(angleStep * i);

                        endValues.x = Mathf.Cos(angleStep * (i + 1));
                        endValues.y = 0.0f;
                        endValues.z = Mathf.Sin(angleStep * (i + 1));
                        break;
                    case DrawingTool.AXIS.X:
                        startValues.x = 0.0f;
                        startValues.y = Mathf.Sin(angleStep * i);
                        startValues.z = Mathf.Cos(angleStep * i);

                        endValues.x = 0.0f;
                        endValues.y = Mathf.Sin(angleStep * (i + 1));
                        endValues.z = Mathf.Cos(angleStep * (i + 1));
                        break;
                    default:
                        break;
                }

                // Set the values
                lineStart = startValues;

                lineEnd = endValues;

                // Results are multiplied so they match the desired spawnRadius
                float lossy = Mathf.Abs(scale.x);
                if (axis == AXIS.Y)
                    lossy = Mathf.Abs(scale.y);
                if (axis == AXIS.Z)
                    lossy = Mathf.Abs(scale.z);

                lineStart *= radius * lossy;
                lineEnd *= radius * lossy;

                // Results are multiplied by the rotation quaternion to rotate them 
                // since this operation is not commutative, result needs to be
                // reassigned, instead of using multiplication assignment operator (*=)
                lineStart = rotation * lineStart;
                lineEnd = rotation * lineEnd;

                // Results are offset by the desired position/origin 
                lineStart += position;
                lineEnd += position;

                // Points are connected using DrawLine method and using the passed color
                Debug.DrawLine(lineStart, lineEnd, secureColor);

                // RESET Gizmos matrix
                Gizmos.matrix = Matrix4x4.identity;
            }
        }
        /// <summary>
        /// Draw a circle around the position based on the spawnRadius, rotation inputs.
        /// </summary>
        /// <param name="position"> Position of the center of the circle </param>
        /// <param name="radius"> Radius of the circle </param>
        /// <param name="color"> Color of the circle </param>
        /// <param name="axis"> Drawing axis </param>
        /// <param name="rotation"> Rotation of the circle </param>
        public static void Circle(Vector3 position, float radius, Color color, DrawingTool.AXIS axis, Quaternion rotation)
        {
            Circle(position, radius, color, axis, rotation, Vector3.one);
        }
        /// <summary>
        /// Draw a circle around the position based on the spawnRadius, rotation inputs.
        /// </summary>
        /// <param name="position"> Position of the center of the circle </param>
        /// <param name="radius"> Radius of the circle </param>
        /// <param name="color"> Color of the circle </param>
        /// <param name="axis"> Drawing axis </param>
        public static void Circle(Vector3 position, float radius, Color color, DrawingTool.AXIS axis = AXIS.Y)
        {
            Circle(position, radius, color, axis, Quaternion.identity);
        }

        // ===== ----- ARC

        /// <summary>
        /// Drawn an arc from 2 angles
        /// </summary>
        /// <param name="startAngle"> Starting angle </param>
        /// <param name="endAngle"> Ending angle for arcSpan </param>
        /// <param name="position"> Position to create the arc </param>
        /// <param name="orientation"> The rotation of the arc </param>
        /// <param name="radius"> The spawnRadius of the arc </param>
        /// <param name="color">The Color of the arc </param>
        /// <param name="drawChord"> Draw the chord </param>
        /// <param name="drawSector"> Draw the sector </param>
        /// <param name="arcSegments"> Number of segments </param>
        protected static void Arc(float startAngle, float endAngle, Vector3 position, Quaternion orientation, float radius, Color color, bool drawChord = false, bool drawSector = false, int arcSegments = 32)
        {
            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            float arcSpan = Mathf.DeltaAngle(startAngle, endAngle);

            // Since Mathf.DeltaAngle returns a signed angle of the shortest path between two angles, it 
            // is necessary to offset it by 360.0 degrees to get a positive value
            if (arcSpan <= 0)
            {
                arcSpan += 360.0f;
            }

            // angle step is calculated by dividing the arc span by number of approximation segments
            float angleStep = (arcSpan / arcSegments) * Mathf.Deg2Rad;
            float stepOffset = startAngle * Mathf.Deg2Rad;

            // stepStart, stepEnd, lineStart and lineEnd variables are declared outside of the following for loop
            float stepStart = 0.0f;
            float stepEnd = 0.0f;
            Vector3 lineStart = Vector3.zero;
            Vector3 lineEnd = Vector3.zero;

            // arcStart and arcEnd need to be stored to be able to draw segment chord
            Vector3 arcStart = Vector3.zero;
            Vector3 arcEnd = Vector3.zero;

            // arcOrigin represents an origin of a circle which defines the arc
            Vector3 arcOrigin = position;

            for (int i = 0; i < arcSegments; i++)
            {
                // Calculate approximation segment start and end, and offset them by start angle
                stepStart = angleStep * i + stepOffset;
                stepEnd = angleStep * (i + 1) + stepOffset;

                lineStart.x = Mathf.Cos(stepStart);
                lineStart.y = Mathf.Sin(stepStart);
                lineStart.z = 0.0f;

                lineEnd.x = Mathf.Cos(stepEnd);
                lineEnd.y = Mathf.Sin(stepEnd);
                lineEnd.z = 0.0f;

                // Results are multiplied so they match the desired spawnRadius
                lineStart *= radius;
                lineEnd *= radius;

                // Results are multiplied by the rotation quaternion to rotate them 
                // since this operation is not commutative, result needs to be
                // reassigned, instead of using multiplication assignment operator (*=)
                lineStart = orientation * lineStart;
                lineEnd = orientation * lineEnd;

                // Results are offset by the desired position/origin 
                lineStart += position;
                lineEnd += position;

                // If this is the first iteration, set the chordStart
                if (i == 0)
                {
                    arcStart = lineStart;
                }

                // If this is the last iteration, set the chordEnd
                if (i == arcSegments - 1)
                {
                    arcEnd = lineEnd;
                }

                Debug.DrawLine(lineStart, lineEnd, secureColor);
            }

            if (drawChord)
            {
                Debug.DrawLine(arcStart, arcEnd, secureColor);
            }
            if (drawSector)
            {
                Debug.DrawLine(arcStart, arcOrigin, secureColor);
                Debug.DrawLine(arcEnd, arcOrigin, secureColor);
            }

        }

        // ===== ----- PATH

        /// <summary>
        /// Draw lines between positions to create a path
        /// </summary>
        /// <param name="positions"> The positions for the path </param>
        /// <param name="color"> Path color </param>
        /// <param name="loop"> Loop or not the path </param>
        /// 
        public static void Path(List<Vector3> positions, Color color, bool loop = false)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = secureColor.a > 0 ? secureColor.a : 1;

            Gizmos.color = secureColor;

            for (int i = 0; i < positions.Count; i++)
            {
                if (i < positions.Count - 1)
                {
                    // DRAW TO NEXT POINT
                    Gizmos.DrawLine(positions[i], positions[i + 1]);
                }
                else if ((i == positions.Count - 1) && loop)
                {
                    // LOOP
                    Gizmos.DrawLine(positions[i], positions[0]);
                }
            }

            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;
        }
        /// <summary>
        /// Draw lines between objects to create a path
        /// </summary>
        /// <param name="relativeObjects"> The objects to get positions for the path </param>
        /// <param name="color"> Path color </param>
        /// <param name="loop"> Loop or not the path </param>
        public static void Path(List<Transform> relativeObjects, Color color, bool loop = false)
        {
            List<Vector3> poses = new List<Vector3>();

            for (int i = 0; i < relativeObjects.Count; i++)
            {
                poses.Add(relativeObjects[i].position);
            }

            Path(poses, color, loop);
        }

        // ===== ----- PATH TENSION

        /// <summary>
        /// Draw lines between positions to create a path with visual tension for minimal and maximal distance
        /// </summary>
        /// <param name="positions"> The positions for the path </param>
        /// <param name="gradient"> Gradient to show the tension </param>
        /// <param name="minDistance"> The minimal distance to show the 1st color </param>
        /// <param name="maxDistance"> The minimal distance to show the last color </param>
        /// <param name="loop"> Loop or not the path </param>
        public static void PathTension(List<Vector3> positions, Gradient gradient, float minDistance = 0, float maxDistance = 0, bool loop = false)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            for (int i = 0; i < positions.Count; i++)
            {
                float distance = 0;
                float tension = 0;

                Vector3 pos1 = Vector3.zero;
                Vector3 pos2 = Vector3.zero;

                if (i < positions.Count - 1)
                {


                    // DRAW TO NEXT POINT

                    pos1 = positions[i];
                    pos2 = positions[i + 1];

                }
                else if ((i == positions.Count - 1) && loop)
                {
                    // LOOP

                    pos1 = positions[i];
                    pos2 = positions[0];

                }

                // Distance between positions
                distance = Vector3.Distance(pos1, pos2);

                // Calculate tension percentage
                tension = (distance - minDistance) / (float)maxDistance;

                // Clamp for Gradient evaluate
                tension = Mathf.Clamp(tension, 0, 1);

                Color secureColor = Color.black;
                if(gradient != null)
                    secureColor = gradient.Evaluate(tension);
                secureColor.a = secureColor.a > 0 ? secureColor.a : 1.0f;

                Gizmos.color = secureColor;

                Gizmos.DrawLine(pos1, pos2);
            }
        }
        /// <summary>
        /// Draw lines between positions to create a path with visual tension for minimal and maximal distance
        /// </summary>
        /// <param name="positions"> The positions for the path </param>
        /// <param name="colors"> Colors to show the tension </param>
        /// <param name="minDistance"> The minimal distance to show the 1st color </param>
        /// <param name="maxDistance"> The minimal distance to show the last color </param>
        /// <param name="loop"> Loop or not the path </param>
        public static void PathTension(List<Vector3> positions, List<Color> colors, float minDistance = 0, float maxDistance = 0, bool loop = false)
        {
            Gradient gradient = CreateGradientFromColors(colors);
            PathTension(positions, gradient, minDistance, maxDistance, loop);
        }

        /// <summary>
        /// Draw lines between objects to create a path with visual tension for minimal and maximal distance
        /// </summary>
        /// <param name="relativeObjects"> The objects to get positions for the path </param>
        /// <param name="gradient"> Colors to show the tension </param>
        /// <param name="minDistance"> The minimal distance to show the 1st color </param>
        /// <param name="maxDistance"> The minimal distance to show the last color </param>
        /// <param name="loop"> Loop or not the path </param>
        public static void PathTension(List<Transform> relativeObjects, Gradient gradient, float minDistance = 0, float maxDistance = 0, bool loop = false)
        {
            List<Vector3> poses = new List<Vector3>();

            for (int i = 0; i < relativeObjects.Count; i++)
            {
                poses.Add(relativeObjects[i].position);
            }

            PathTension(poses, gradient, minDistance, maxDistance, loop);
        }
        /// <summary>
        /// Draw lines between objects to create a path with visual tension for minimal and maximal distance
        /// </summary>
        /// <param name="relativeObjects"> The objects to get positions for the path </param>
        /// <param name="colors"> Colors to show the tension </param>
        /// <param name="minDistance"> The minimal distance to show the 1st color </param>
        /// <param name="maxDistance"> The minimal distance to show the last color </param>
        /// <param name="loop"> Loop or not the path </param>
        public static void PathTension(List<Transform> relativeObjects, List<Color> colors, float minDistance = 0, float maxDistance = 0, bool loop = false)
        {
            List<Vector3> poses = new List<Vector3>();

            for(int i = 0; i < relativeObjects.Count; i++)
            {
                poses.Add(relativeObjects[i].position);
            }

            PathTension(poses, colors, minDistance, maxDistance, loop);
        }

        // ----- COLORS GRADIENT CONVERTER

        /// <summary>
        /// Get a gradient made from the colors inputs
        /// </summary>
        /// <param name="gradientColors"></param>
        /// <returns></returns>
        protected static Gradient CreateGradientFromColors(List<Color> gradientColors)
        {
            var gradient = new Gradient();

            if (gradientColors != null)
            {
                int nbrColors = 0;
                List<Color> colors = new List<Color>();

                if (gradientColors.Count > 1)
                {
                    nbrColors = gradientColors.Count;
                    colors = gradientColors;
                }
                else if (gradientColors.Count == 1)
                {
                    nbrColors = 2;
                    colors = gradientColors;
                    colors.Add(gradientColors[0]);
                }
                else
                {
                    nbrColors = 2;
                    colors.Add(Color.white);
                    colors.Add(Color.white);
                }

                // Create as many keys as colors
                var gradColors = new GradientColorKey[nbrColors];
                var gradAlphas = new GradientAlphaKey[nbrColors];

                for (int i = 0; i < nbrColors; i++)
                {
                    float time = i / (nbrColors - 1);
                    gradColors[i] = new GradientColorKey(colors[i], time);
                    gradAlphas[i] = new GradientAlphaKey(colors[i].a, time);
                }


                gradient.SetKeys(gradColors, gradAlphas);
            }

            return gradient;
        }

        // ===== ----- BOX

        /// <summary>
        /// Draw a box at the position based on a rotation, scale and size.
        /// </summary>
        /// <param name="position"> Where to create the box </param>
        /// <param name="scale"> Scale of the box </param>
        /// <param name="size"> Size of the box </param>
        /// <param name="color"> Color of the box </param>
        /// <param name="rotation"> Rotation of the box </param>
        /// <param name="isWire"> Draw it as wire </param>
        public static void Box(Vector3 position, Vector3 size, Color color, Quaternion rotation, Vector3 scale, bool isWire = false)
        {
            // SET Gizmos matrix
            var matrix = Matrix4x4.TRS(position, rotation, scale);
            Gizmos.matrix = matrix;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = secureColor.a > 0 ? secureColor.a : 1;

            Gizmos.color = secureColor;            

            if (isWire)
                Gizmos.DrawWireCube(Vector3.zero, size);
            else
                Gizmos.DrawCube(Vector3.zero, size);

            Gizmos.matrix = Matrix4x4.identity;
        }
        /// <summary>
        /// Draw a box at the position based on size.
        /// </summary>
        /// <param name="position"> Where to create the box </param>
        /// <param name="size"> Size of the box </param>
        /// <param name="color"> Color of the box </param>
        public static void Box(Vector3 position, Vector3 size, Color color)
        {
            DebugDraw.Box(position, size, color, Quaternion.identity, Vector3.one);
        }

        /// <summary>
        /// Draw a wire box at the position based on a rotation, scale and size.
        /// </summary>
        /// <param name="position"> Where to create the box </param>
        /// <param name="rotation"> Rotation of the box </param>
        /// <param name="scale"> Scale of the box </param>
        /// <param name="size"> Size of the box </param>
        /// <param name="color"> Color of the box </param>
        public static void WireBox(Vector3 position, Vector3 size, Color color, Quaternion rotation, Vector3 scale)
        {
            DebugDraw.Box(position, size, color, rotation, scale, true);
        }
        /// <summary>
        /// Draw a wire box at the position based on size.
        /// </summary>
        /// <param name="position"> Where to create the box </param>
        /// <param name="size"> Size of the box </param>
        /// <param name="color"> Color of the box </param>
        public static void WireBox(Vector3 position, Vector3 size, Color color)
        {
            DebugDraw.WireBox(position, size, color, Quaternion.identity, Vector3.one);
        }


        // ===== ----- SPHERE

        /// <summary>
        /// Draw a sphere at the position based on a scale and spawnRadius.
        /// </summary>
        /// <param name="position"> Where to create the sphere </param>
        /// <param name="radius"> Radius of the sphere </param>
        /// <param name="rotation"> Rotation of the sphere </param>
        /// <param name="scale"> Scale of the sphere </param>
        /// <param name="color"> Color of the sphere </param>
        /// <param name="isWire"> Draw it as wire </param>
        public static void Sphere(Vector3 position, float radius, Color color, Quaternion rotation, Vector3 scale, bool isWire = false)
        {
            Vector3 lossy = scale;
            float max = Mathf.Abs(lossy.x);
            if (max < Mathf.Abs(scale.y))
                max = Mathf.Abs(lossy.y);
            if (max < Mathf.Abs(scale.z))
                max = Mathf.Abs(lossy.z);

            lossy.x = max;
            lossy.y = max;
            lossy.z = max;

            // SET Gizmos matrix
            var matrix = Matrix4x4.TRS(position, rotation, lossy);
            Gizmos.matrix = matrix;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            if (isWire)
                Gizmos.DrawWireSphere(Vector3.zero, radius);
            else
                Gizmos.DrawSphere(Vector3.zero, radius);

            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;
        }
        /// <summary>
        /// Draw a sphere at the position based on a scale and spawnRadius.
        /// </summary>
        /// <param name="position"> Where to create the sphere </param>
        /// <param name="radius"> Radius of the sphere </param>
        /// <param name="color"> Color of the sphere </param>
        public static void Sphere(Vector3 position, float radius, Color color)
        {
            DebugDraw.Sphere(position, radius, color, Quaternion.identity, Vector3.one);
        }

        /// <summary>
        /// Draw a wire sphere at the position based on a scale and spawnRadius.
        /// </summary>
        /// <param name="position"> Where to create the sphere </param>
        /// <param name="radius"> Radius of the sphere </param>
        /// <param name="scale"> Scale of the sphere </param>
        /// <param name="color"> Color of the sphere </param>
        public static void WireSphere(Vector3 position, float radius, Color color, Quaternion rotation, Vector3 scale)
        {
            DebugDraw.Sphere(position, radius, color, rotation, scale, true);
        }
        /// <summary>
        /// Draw a wire sphere at the position based on a scale and spawnRadius.
        /// </summary>
        /// <param name="position"> Where to create the sphere </param>
        /// <param name="radius"> Radius of the sphere </param>
        /// <param name="color"> Color of the sphere </param>
        public static void WireSphere(Vector3 position, float radius, Color color)
        {
            DebugDraw.WireSphere(position, radius,color, Quaternion.identity, Vector3.one);
        }


        // ===== ----- CYLINDER

        /// <summary>
        /// Draw a wire cylinder based on a rotation, height and spawnRadius
        /// </summary>
        /// <param name="position"> The position where to create the cylinder </param>
        /// <param name="rotation"> The rotation of the cylinder </param>
        /// <param name="height"> The height of the cylinder </param>
        /// <param name="radius"> The spawnRadius of the cylinder </param>
        /// <param name="color"> The color of the cylinder </param>
        /// <param name="drawFromBase"> If the cylinder is drawn from the base </param>
        public static void WireCylinder(Vector3 position, float height, float radius, Color color, DrawingTool.AXIS axis, Quaternion rotation)
        {
            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            // Local direction
            Vector3 localUp = rotation * Vector3.up;
            Vector3 localRight = rotation * Vector3.right;
            Vector3 localForward = rotation * Vector3.forward;

            switch (axis)
            {
                case AXIS.X:
                    localUp = rotation * Vector3.right;
                    localRight = rotation * Vector3.up;
                    localForward = rotation * Vector3.forward;
                    break;
                case AXIS.Z:
                    localUp = rotation * Vector3.forward;
                    localRight = rotation * Vector3.right;
                    localForward = rotation * Vector3.up;
                    break;
            }

            // Center of the cylinder
            Vector3 basePositionOffset = (localUp * height * 0.5f);

            // Bottom and top center
            Vector3 basePosition = position - basePositionOffset;
            Vector3 topPosition = basePosition + localUp * height;

            Quaternion circleOrientation = rotation * Quaternion.Euler(90, 0, 0);

            Vector3 pointA = basePosition + localRight * radius;
            Vector3 pointB = basePosition + localForward * radius;
            Vector3 pointC = basePosition - localRight * radius;
            Vector3 pointD = basePosition - localForward * radius;

            Debug.DrawRay(pointA, localUp * height, secureColor);
            Debug.DrawRay(pointB, localUp * height, secureColor);
            Debug.DrawRay(pointC, localUp * height, secureColor);
            Debug.DrawRay(pointD, localUp * height, secureColor);


            DebugDraw.Circle(basePosition, radius, secureColor, axis, rotation);
            DebugDraw.Circle(topPosition, radius, secureColor, axis, rotation);
        }


        // ===== ----- CAPSULE

        /// <summary>
        /// Draw a wire capsule based on a rotation, height and spawnRadius
        /// </summary>
        /// <param name="position"> The position where to create the capsule </param>
        /// <param name="height"> The height of the capsule </param>
        /// <param name="radius"> The spawnRadius of the capsule </param>
        /// <param name="color"> The color of the capsule </param>
        /// <param name="axis"> Drawing axis </param>
        /// <param name="rotation"> The rotation of the capsule </param>
        /// <param name="scale"> Scale of the capsule </param>
        public static void WireCapsule(Vector3 position, float height, float radius, Color color, DrawingTool.AXIS axis, Quaternion rotation, Vector3 scale)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            float lossyRadius = 1;
            float lossyHeight = 1;

            switch (axis)
            {
                case AXIS.X:
                    lossyRadius = scale.x < scale.z ? scale.z : scale.x;
                    lossyHeight = scale.y;
                    break;
                case AXIS.Y:
                    lossyRadius = scale.x < scale.z ? scale.z : scale.x;
                    lossyHeight = scale.y;
                    break;
                case AXIS.Z:
                    lossyRadius = scale.x < scale.z ? scale.z : scale.x;
                    lossyHeight = scale.y;
                    break;
            }

            height = lossyHeight * height;
            radius = lossyRadius * radius;


            if (height <= radius * 2)
            {
                DebugDraw.WireSphere(position, radius, color);
            }
            else
            {
                // Clamp the spawnRadius to a half of the capsule's height
                radius = Mathf.Clamp(radius, 0, height * 0.5f);

                Vector3 localUp = rotation * Vector3.up;
                Quaternion arcOrientation = rotation * Quaternion.Euler(0, 90, 0);
                Quaternion newRotation = rotation;
                float angleAxis = 0;
                float otherAngleAxis = 0;
                // Local direction
                switch (axis)
                {
                    case AXIS.X:
                        localUp = rotation * Vector3.right;
                        arcOrientation = rotation * Quaternion.Euler(90, 0, 0);
                        angleAxis = -90;
                        otherAngleAxis = -90;
                        break;
                    case AXIS.Z:
                        localUp = rotation * Vector3.forward;
                        arcOrientation = rotation * Quaternion.Euler(0, 90, 0);
                        newRotation *= Quaternion.Euler(90, 0, 0);
                        angleAxis = 0;
                        otherAngleAxis = 90;
                        break;
                }

                
                Vector3 capsuleCenter = (localUp * height * 0.5f);

                Vector3 baseArcPosition = position + localUp * radius - capsuleCenter;


                DebugDraw.Arc(180 + angleAxis, 360 + angleAxis, baseArcPosition, newRotation, radius, secureColor);
                DebugDraw.Arc(180 + otherAngleAxis, 360 + otherAngleAxis, baseArcPosition, arcOrientation, radius, secureColor);



                float cylinderHeight = height - radius * 2.0f;

                DebugDraw.WireCylinder(position, cylinderHeight, radius, secureColor, axis, rotation);

                Vector3 topArcPosition = baseArcPosition + localUp * cylinderHeight;

                DebugDraw.Arc(0 + angleAxis, 180 + angleAxis, topArcPosition, newRotation, radius, secureColor);
                DebugDraw.Arc(0 + otherAngleAxis, 180 + otherAngleAxis, topArcPosition, arcOrientation, radius, secureColor);

            }

        }


        // ===== ----- MESH

        /// <summary>
        /// Draw a mesh based on a mesh, rotation and scale
        /// </summary>
        /// <param name="position"> Position of the mesh </param>
        /// <param name="mesh"> Mesh drawn </param>
        /// <param name="color"> Color of the mesh </param>
        /// <param name="rotation"> Rotation of the mesh </param>
        /// <param name="scale"> Scale of the mesh </param>
        /// <param name="isWire"> Draw it as a wire </param>
        public static void Mesh(Vector3 position, Mesh mesh, Color color, Quaternion rotation, Vector3 scale,  bool isWire = false)
        {
            Gizmos.matrix = Matrix4x4.identity;

            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            if (isWire)
                Gizmos.DrawWireMesh(mesh, -1, position, rotation, scale);
            else
                Gizmos.DrawMesh(mesh, -1, position, rotation, scale);
        }

        /// <summary>
        /// Draw a mesh based on a mesh, rotation and scale
        /// </summary>
        /// <param name="position"> Position of the mesh </param>
        /// <param name="mesh"> Mesh drawn </param>
        /// <param name="color"> Color of the mesh </param>
        /// <param name="rotation"> Rotation of the mesh </param>
        /// <param name="scale"> Scale of the mesh </param>
        public static void WireMesh(Vector3 position, Mesh mesh, Color color, Quaternion rotation, Vector3 scale)
        {
            DebugDraw.Mesh(position, mesh, color, rotation, scale,  true);
        }


        // ==================== ----- COLLIDERS
        ///
        // ===== ----- BOX

        /// <summary>
        /// Draw the box of a BoxCollider
        /// </summary>
        /// <param name="col"> BoxCollider drawn </param>
        /// <param name="color"> Color of the BoxCollier </param>
        /// <param name="isWire"> Draw it as wire </param>
        public static void BoxCollider(BoxCollider col, Color color, bool isWire = false)
        {
            Vector3 pos = col.center;
            pos.x *= col.transform.lossyScale.x;
            pos.y *= col.transform.lossyScale.y;
            pos.z *= col.transform.lossyScale.z;

            if (isWire)
                DebugDraw.WireBox(col.transform.position + col.transform.rotation * pos, col.size, color, col.transform.rotation, col.transform.lossyScale);
            else
                DebugDraw.Box(col.transform.position + col.transform.rotation * pos, col.size, color, col.transform.rotation, col.transform.lossyScale);
        }

        /// <summary>
        /// Draw the wire box of a BoxCollider
        /// </summary>
        /// <param name="col"> BoxCollider drawn </param>
        /// <param name="color"> Color of the BoxCollier </param>
        public static void WireBoxCollider(BoxCollider col, Color color)
        {
            DebugDraw.BoxCollider(col, color, true);
        }


        // ===== ----- SPHERE

        /// <summary>
        /// Draw the sphere of a SphereCollider
        /// </summary>
        /// <param name="col"> SphereCollider drawn </param>
        /// <param name="color"> Color of the SphereCollider </param>
        /// <param name="isWire"> Draw it as wire </param>
        public static void SphereCollider(SphereCollider col, Color color, bool isWire = false)
        {
            Vector3 pos = col.center;
            pos.x *= col.transform.lossyScale.x;
            pos.y *= col.transform.lossyScale.y;
            pos.z *= col.transform.lossyScale.z;

            if (isWire)
                DebugDraw.WireSphere(col.transform.position + col.transform.rotation * pos, col.radius, color, col.transform.rotation, col.transform.lossyScale);
            else
                DebugDraw.Sphere(col.transform.position + col.transform.rotation * pos, col.radius, color, col.transform.rotation, col.transform.lossyScale);
        }
        
        /// <summary>
        /// Draw the wire sphere of a SphereCollider
        /// </summary>
        /// <param name="col"> SphereCollider drawn </param>
        /// <param name="color"> Color of the SphereCollider </param>
        public static void WireSphereCollider(SphereCollider col, Color color)
        {
            DebugDraw.SphereCollider(col, color, true);
        }


        // ===== ----- CAPSULE

        /// <summary>
        /// Draw the wire capsule of a CapsuleCollider
        /// </summary>
        /// <param name="col"> CapsuleCollider drawn </param>
        /// <param name="color"> Color of the CapsuleCollider </param>
        public static void WireCapsuleCollider(CapsuleCollider col, Color color)
        {
            Gizmos.matrix = Matrix4x4.identity;

            Vector3 center = col.center;
            center.x *= col.transform.lossyScale.x;
            center.y *= col.transform.lossyScale.y;
            center.z *= col.transform.lossyScale.z;

            Vector3 position = col.transform.position + col.transform.rotation * center;

            switch (col.direction)
            {
                case 0:
                    DebugDraw.WireCapsule(position, col.height, col.radius, color, DrawingTool.AXIS.X,col.transform.rotation, col.transform.lossyScale);
                    break;
                case 1:
                    DebugDraw.WireCapsule(position, col.height, col.radius, color, DrawingTool.AXIS.Y, col.transform.rotation, col.transform.lossyScale);
                    break;
                case 2:
                    DebugDraw.WireCapsule(position, col.height, col.radius, color, DrawingTool.AXIS.Z, col.transform.rotation, col.transform.lossyScale);
                    break;
            }

        }


        // ===== ----- MESH

        /// <summary>
        /// Draw the mesh of a MeshCollider
        /// </summary>
        /// <param name="col"> MeshCollider drawn </param>
        /// <param name="color"> Color of the MeshCollider </param>
        public static void MeshCollider(MeshCollider col, Color color, bool isWire = false)
        {
            if (isWire)
                DebugDraw.WireMesh(col.transform.position, col.sharedMesh, color, col.transform.rotation, col.transform.lossyScale);
            else
                DebugDraw.Mesh(col.transform.position, col.sharedMesh, color, col.transform.rotation, col.transform.lossyScale);

        }

        /// <summary>
        /// Draw the wire mesh of a MeshCollider
        /// </summary>
        /// <param name="col"> MeshCollider drawn </param>
        /// <param name="color"> Color of the MeshCollider </param>
        public static void WireMeshCollider(MeshCollider col, Color color)
        {
            DebugDraw.MeshCollider(col, color, true);
        }


        // ===== ----- COLLIDER

        /// <summary>
        /// Draw any collider as a wire
        /// </summary>
        /// <param name="col"> Collider drawn </param>
        /// <param name="color"> Color of the Collider </param>
        public static void WireCollider(Collider col, Color color)
        {
            if (typeof(BoxCollider) == col.GetType())
            {
                DebugDraw.WireBoxCollider(col as BoxCollider, color);
            }
            else if (typeof(SphereCollider) == col.GetType())
            {
                DebugDraw.WireSphereCollider(col as SphereCollider, color);
            }
            else if (typeof(CapsuleCollider) == col.GetType())
            {
                DebugDraw.WireCapsuleCollider(col as CapsuleCollider, color);
            }
            else if (typeof(MeshCollider) == col.GetType())
            {
                DebugDraw.WireMeshCollider(col as MeshCollider, color);
            }
            else
                return;
        }

    }

}
