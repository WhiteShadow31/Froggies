using System.Collections.Generic;
using UnityEngine;


namespace DrawingTool
{
    public class DebugDraw2D 
    {
        // ==================== ----- DRAWING
        ///
        // ===== ----- PATH

        /// <summary>
        /// Draw lines between objects to create a path
        /// </summary>
        /// <param name="positions"> The positions for the path </param>
        /// <param name="color"> Path color </param>
        /// <param name="loop"> Loop or not the path </param>
        /// 
        public static void Path2D(List<Vector2> positions, Color color, bool loop = false)
        {
            Gizmos.matrix = Matrix4x4.identity;

            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

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
        }
        /// <summary>
        /// Draw lines between objects to create a path
        /// </summary>
        /// <param name="relativeObjects"> The objects to get positions for the path </param>
        /// <param name="color"> Path color </param>
        /// <param name="loop"> Loop or not the path </param>
        public static void Path2D(List<Transform> relativeObjects, Color color, bool loop = false)
        {
            List<Vector2> poses = new List<Vector2>();

            for (int i = 0; i < relativeObjects.Count; i++)
            {
                poses.Add(relativeObjects[i].position);
            }

            Path2D(poses, color, loop);
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
        public static void PathTension2D(List<Vector2> positions, Gradient gradient, float minDistance = 0, float maxDistance = 0, bool loop = false)
        {
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
                if (gradient != null)
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
        public static void PathTension2D(List<Vector2> positions, List<Color> colors, float minDistance = 0, float maxDistance = 0, bool loop = false)
        {
            Gradient gradient = CreateGradientFromColors(colors);
            PathTension2D(positions, gradient, minDistance, maxDistance, loop);
        }

        /// <summary>
        /// Draw lines between objects to create a path with visual tension for minimal and maximal distance
        /// </summary>
        /// <param name="relativeObjects"> The objects to get positions for the path </param>
        /// <param name="gradient"> Colors to show the tension </param>
        /// <param name="minDistance"> The minimal distance to show the 1st color </param>
        /// <param name="maxDistance"> The minimal distance to show the last color </param>
        /// <param name="loop"> Loop or not the path </param>
        public static void PathTension2D(List<Transform> relativeObjects, Gradient gradient, float minDistance = 0, float maxDistance = 0, bool loop = false)
        {
            List<Vector2> poses = new List<Vector2>();

            for (int i = 0; i < relativeObjects.Count; i++)
            {
                poses.Add(relativeObjects[i].position);
            }

            PathTension2D(poses, gradient, minDistance, maxDistance, loop);
        }
        /// <summary>
        /// Draw lines between objects to create a path with visual tension for minimal and maximal distance
        /// </summary>
        /// <param name="relativeObjects"> The objects to get positions for the path </param>
        /// <param name="colors"> Colors to show the tension </param>
        /// <param name="minDistance"> The minimal distance to show the 1st color </param>
        /// <param name="maxDistance"> The minimal distance to show the last color </param>
        /// <param name="loop"> Loop or not the path </param>
        public static void PathTension2D(List<Transform> relativeObjects, List<Color> colors, float minDistance = 0, float maxDistance = 0, bool loop = false)
        {
            List<Vector2> poses = new List<Vector2>();

            for (int i = 0; i < relativeObjects.Count; i++)
            {
                poses.Add(relativeObjects[i].position);
            }

            PathTension2D(poses, colors, minDistance, maxDistance, loop);
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

        // ===== ----- ARC

        /// <summary>
        /// Drawn an arc from 2 angles
        /// </summary>
        /// <param name="startAngle"> Starting angle </param>
        /// <param name="endAngle"> Ending angle for arcSpan </param>
        /// <param name="position"> Position to create the arc </param>
        /// <param name="orientation"> The orientation of the arc </param>
        /// <param name="radius"> The spawnRadius of the arc </param>
        /// <param name="color">The Color of the arc </param>
        /// <param name="drawChord"> Draw the chord </param>
        /// <param name="drawSector"> Draw the sector </param>
        /// <param name="arcSegments"> Number of segments </param>
        protected static void Arc2D(float startAngle, float endAngle, Vector2 position, Quaternion orientation, float radius, Color color, bool drawChord = false, bool drawSector = false, int arcSegments = 32)
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
            Vector2 lineStart = Vector3.zero;
            Vector2 lineEnd = Vector3.zero;

            // arcStart and arcEnd need to be stored to be able to draw segment chord
            Vector2 arcStart = Vector3.zero;
            Vector2 arcEnd = Vector3.zero;

            // arcOrigin represents an origin of a circle which defines the arc
            Vector3 arcOrigin = position;

            for (int i = 0; i < arcSegments; i++)
            {
                // Calculate approximation segment start and end, and offset them by start angle
                stepStart = angleStep * i + stepOffset;
                stepEnd = angleStep * (i + 1) + stepOffset;

                lineStart.x = Mathf.Cos(stepStart);
                lineStart.y = Mathf.Sin(stepStart);

                lineEnd.x = Mathf.Cos(stepEnd);
                lineEnd.y = Mathf.Sin(stepEnd);

                // Results are multiplied so they match the desired spawnRadius
                lineStart *= radius;
                lineEnd *= radius;

                // Results are multiplied by the orientation quaternion to rotate them 
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

        // ===== ----- CIRCLE

        /// <summary>
        /// Draw a circle at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the circle </param>
        /// <param name="radius"> Radius of the circle </param>
        /// <param name="color"> Color of the circle </param>
        /// <param name="rotation"> Rotation of the circle </param>
        /// <param name="scale"> Scale of the circle </param>
        /// <param name="nbrOfPoints"> Number of points to draw the circle </param>
        public static void Circle2D(Vector2 position, float radius, Color color, Quaternion rotation, Vector3 scale, int nbrOfPoints = 45)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = secureColor.a > 0 ? secureColor.a : 1;

            Gizmos.color = secureColor;

            // Single segment of the circle covers (360 / number of segments) degrees
            float angleStep = (360.0f / nbrOfPoints);

            // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
            // which are required by Unity's Mathf class trigonometry methods

            angleStep *= Mathf.Deg2Rad;

            // lineStart and lineEnd variables are declared outside of the following for loop
            Vector2 lineStart = Vector3.zero;
            Vector2 lineEnd = Vector3.zero;

            for (int i = 0; i < nbrOfPoints; i++)
            {
                Vector2 startValues = Vector2.zero;
                Vector2 endValues = Vector2.zero;


                startValues.x = Mathf.Cos(angleStep * i);
                startValues.y = Mathf.Sin(angleStep * i);

                // Path end is defined by the angle of the next segment (i+1)
                endValues.x = Mathf.Cos(angleStep * (i + 1));
                endValues.y = Mathf.Sin(angleStep * (i + 1));

                // Set the values
                lineStart = startValues;

                lineEnd = endValues;

                // Results are multiplied so they match the desired spawnRadius
                float lossy = Mathf.Abs(scale.x);
                if (lossy < Mathf.Abs(scale.y))
                    lossy = Mathf.Abs(scale.y);

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
            }

            // RESET Gizmos matrix for next draw (outside this tool)
            Gizmos.matrix = Matrix4x4.identity;
        }
        /// <summary>
        /// Draw a circle at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the circle </param>
        /// <param name="radius"> Radius of the circle </param>
        /// <param name="color"> Color of the circle </param>
        /// <param name="rotation"> Rotation of the circle </param>
        public static void Circle2D(Vector2 position, float radius, Color color, Quaternion rotation)
        {
            DebugDraw2D.Circle2D(position, radius, color, rotation, Vector3.one);
        }
        /// <summary>
        /// Draw a circle at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the circle </param>
        /// <param name="radius"> Radius of the circle </param>
        /// <param name="color"> Color of the circle </param>
        public static void Circle2D(Vector2 position, float radius, Color color)
        {
            DebugDraw2D.Circle2D(position, radius, color, Quaternion.identity);
        }

        // ===== ----- SQUARE / BOX

        /// <summary>
        /// Draw a square at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the square </param>
        /// <param name="size"> Size of the square </param>
        /// <param name="color"> Color of the square </param>
        /// <param name="rotation"> Rotation of the square </param>
        /// <param name="scale"> Scale of the square </param>
        public static void Square2D(Vector2 position, Vector2 size, Color color, Quaternion rotation, Vector3 scale)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            // Calculate size with scale
            Vector2 scaleSize = size;
            scaleSize.x *= scale.x;
            scaleSize.y *= scale.y;

            // The four corner of the box
            Vector2 rightUpCorner = Vector2.zero;
            Vector2 rightDownCorner = Vector2.zero;
            Vector2 leftDownCorner = Vector2.zero;
            Vector2 leftUpCorner = Vector2.zero;

            // Right Down corner
            rightDownCorner.x = scaleSize.x / 2;
            rightDownCorner.y = -scaleSize.y / 2;

            // Right Up
            rightUpCorner.x = scaleSize.x / 2;
            rightUpCorner.y = scaleSize.y / 2;

            // Left Down corner
            leftDownCorner.x = -scaleSize.x / 2;
            leftDownCorner.y = -scaleSize.y / 2;

            // Left Up corner
            leftUpCorner.x = -scaleSize.x / 2;
            leftUpCorner.y = scaleSize.y / 2;

            // Take in accompt the rotation
            rightUpCorner = rotation * rightUpCorner;
            rightDownCorner = rotation * rightDownCorner;
            leftDownCorner = rotation * leftDownCorner;
            leftUpCorner = rotation * leftUpCorner;

            // Set it as the position inputed
            rightUpCorner += position;
            rightDownCorner += position;
            leftDownCorner += position;
            leftUpCorner += position;

            // Draw
            Debug.DrawLine(rightUpCorner, rightDownCorner, secureColor);
            Debug.DrawLine(rightDownCorner, leftDownCorner, secureColor);
            Debug.DrawLine(leftDownCorner, leftUpCorner, secureColor);
            Debug.DrawLine(leftUpCorner, rightUpCorner, secureColor);

            // RESET Gizmos matrix for next draw (outside this tool)
            Gizmos.matrix = Matrix4x4.identity;
        }
        /// <summary>
        /// Draw a square at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the square </param>
        /// <param name="size"> Size of the square </param>
        /// <param name="color"> Color of the square </param>
        /// <param name="rotation"> Rotation of the square </param>
        public static void Square2D(Vector2 position, Vector2 size, Color color, Quaternion rotation)
        {
            DebugDraw2D.Square2D(position, size, color, rotation, Vector3.one);
        }
        /// <summary>
        /// Draw a square at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the square </param>
        /// <param name="size"> Size of the square </param>
        /// <param name="color"> Color of the square </param>
        public static void Square2D(Vector2 position, Vector2 size, Color color)
        {
            DebugDraw2D.Square2D(position, size, color, Quaternion.identity, Vector3.one);
        }

        // ===== ----- CAPSULE

        /// <summary>
        /// Draw a capsule at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the capsule </param>
        /// <param name="size"> Size of the capsule </param>
        /// <param name="color"> Color of the capsule </param>
        /// <param name="rotation"> Rotation of the capsule </param>
        /// <param name="scale"> Scale of the capsule </param>
        public static void Capsule2D(Vector2 position, Vector2 size, Color color, DrawingTool.AXIS axis, Quaternion rotation, Vector3 scale)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            // Axis can't be directed to Z axis
            axis = axis == DrawingTool.AXIS.Z ? DrawingTool.AXIS.Y : axis;

            // Determine what is spawnRadius and height scale based on axis
            float lossyRadius = axis == DrawingTool.AXIS.Y ? scale.x : scale.y;
            float lossyHeight = axis == DrawingTool.AXIS.Y ? scale.y : scale.x;

            // Diameter of the capsule based on it's direction axis
            float diameter = (axis == DrawingTool.AXIS.Y ? size.x : size.y) * lossyRadius;

            // Height of the capsule based on it's direction axis
            float height = (axis == DrawingTool.AXIS.Y ? size.y : size.x) * lossyHeight;

            float radius = diameter / 2;

            // Height capsule is inferior to diameter so it's a sphere
            if (height <= diameter)
            {
                
                DebugDraw2D.Circle2D(position, radius / lossyRadius, color, rotation, scale);
            }
            else
            {
                // Clamp the spawnRadius to a half of the capsule's height
                diameter = Mathf.Clamp(radius, 0, height * 0.5f);

                // Local direction
                Vector2 localUp = axis == DrawingTool.AXIS.Y ? rotation * Vector2.up : rotation * Vector2.right;
                Vector2 localRight = axis == DrawingTool.AXIS.Y ? rotation * Vector2.right : rotation * Vector2.up;

                // Center of the capsule
                Vector2 capsuleCenter = (localUp * height * 0.5f);

                // Height of the capsule minus the half circle at top and bottom
                float cylinderHeight = height - radius * 2.0f;
                
                // Bot and top arc center
                Vector2 baseArcPosition = position + localUp * radius - capsuleCenter;
                Vector3 topArcPosition = baseArcPosition + localUp * cylinderHeight;
                
                // Rotate angle if not vertical
                float angleAxis = axis == DrawingTool.AXIS.Y ? 0 : -90;
                // Draw bottom arc
                DebugDraw2D.Arc2D(180 + angleAxis, 360 + angleAxis, baseArcPosition, rotation, radius, color);
                // Draw top arc
                DebugDraw2D.Arc2D(0 + angleAxis, 180 + angleAxis, topArcPosition, rotation, radius, color);

                // Rays for the interio cylinder
                Vector2 pointRight = baseArcPosition + localRight * radius;
                Vector2 pointLeft = baseArcPosition - localRight * radius;

                Debug.DrawRay(pointRight, localUp * cylinderHeight, secureColor);
                Debug.DrawRay(pointLeft, localUp * cylinderHeight, secureColor);


                // RESET Gizmos matrix for next draw (outside this tool)
                Gizmos.matrix = Matrix4x4.identity;
            }
        }
        /// <summary>
        /// Draw a capsule at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the capsule </param>
        /// <param name="size"> Size of the capsule </param>
        /// <param name="color"> Color of the capsule </param>
        /// <param name="rotation"> Rotation of the capsule </param>
        public static void Capsule2D(Vector2 position, Vector2 size, Color color, DrawingTool.AXIS axis, Quaternion rotation)
        {
            DebugDraw2D.Capsule2D(position, size, color, axis, rotation, Vector3.one);
        }
        /// <summary>
        /// Draw a capsule at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the capsule </param>
        /// <param name="size"> Size of the capsule </param>
        /// <param name="color"> Color of the capsule </param>
        public static void Capsule2D(Vector2 position, Vector2 size, Color color, DrawingTool.AXIS axis = AXIS.Y)
        {
            DebugDraw2D.Capsule2D(position, size, color, axis, Quaternion.identity, Vector3.one);
        }

        // ===== ----- POLYGON

        /// <summary>
        /// Draw a polygon at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the polygon </param>
        /// <param name="points"> Vertex of the polygon </param>
        /// <param name="color"> Color of the polygon </param>
        /// <param name="rotation"> Rotation of the polygon </param>
        /// <param name="scale"> Scale of the polygon </param>
        public static void Polygon2D(Vector2 position, Vector2[] points, Color color, Quaternion rotation, Vector3 scale)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            // Copy the points
            Vector2[] positions = points;

            // Set the positions of the points
            for (int j = 0; j < positions.Length; j++)
            {
                // Position based on rotation
                positions[j] = rotation * (positions[j] * scale);
                positions[j] += position;
            }

            // Draw the points and loop it
            for (int i = 0; i < positions.Length; i++)
            {
                if (i < positions.Length - 1)
                {
                    Debug.DrawLine(positions[i], positions[i + 1], secureColor);
                }
                else
                {
                    Debug.DrawLine(positions[i], positions[0], secureColor);
                }
            }
        
        }
        /// <summary>
        /// Draw a polygon at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the polygon </param>
        /// <param name="points"> Vertex of the polygon </param>
        /// <param name="color"> Color of the polygon </param>
        /// <param name="rotation"> Rotation of the polygon </param>
        public static void Polygon2D(Vector2 position, Vector2[] points, Color color, Quaternion rotation)
        {
            DebugDraw2D.Polygon2D(position, points, color, rotation, Vector3.one);
        }
        /// <summary>
        /// Draw a polygon at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the polygon </param>
        /// <param name="points"> Vertex of the polygon </param>
        /// <param name="color"> Color of the polygon </param>
        public static void Polygon2D(Vector2 position, Vector2[] points, Color color)
        {
            DebugDraw2D.Polygon2D(position, points, color, Quaternion.identity, Vector3.one);
        }

        // ===== ----- EDGE

        /// <summary>
        /// Draw an edge at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the edge </param>
        /// <param name="points"> Vertex of the edge </param>
        /// <param name="color"> Color of the edge </param>
        /// <param name="rotation"> Rotation of the edge </param>
        /// <param name="scale"> Scale of the edge </param>
        public static void Edge2D(Vector2 position, Vector2[] points, Color color, Quaternion rotation, Vector3 scale)
        {
            // RESET Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            // Set the alpha of the color to 1 if it's 0
            Color secureColor = color;
            secureColor.a = color.a > 0 ? color.a : 1.0f;

            Gizmos.color = secureColor;

            // Copy the points
            Vector2[] positions = points;

            // Set the positions of the points
            for (int j = 0; j < positions.Length; j++)
            {
                // Position based on rotation
                positions[j] = rotation * (positions[j] * scale);
                positions[j] += position;
            }

            // Draw the points
            for (int i = 0; i < positions.Length; i++)
            {
                if (i < positions.Length - 1)
                {
                    Debug.DrawLine(positions[i], positions[i + 1], secureColor);
                }
            }
        }
        /// <summary>
        /// Draw an edge at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the edge </param>
        /// <param name="points"> Edges of the edge </param>
        /// <param name="color"> Color of the edge </param>
        /// <param name="rotation"> Rotation of the edge </param>
        public static void Edge2D(Vector2 position, Vector2[] points, Color color, Quaternion rotation)
        {
            DebugDraw2D.Edge2D(position, points, color, rotation, Vector3.one);
        }
        /// <summary>
        /// Draw an edge at a position in 2D space, rotate and scale it if necessary
        /// </summary>
        /// <param name="position"> Position of the center of the edge </param>
        /// <param name="points"> Edges of the edge </param>
        /// <param name="color"> Color of the edge </param>
        public static void Edge2D(Vector2 position, Vector2[] points, Color color)
        {
            DebugDraw2D.Edge2D(position, points, color, Quaternion.identity, Vector3.one);
        }



        // ==================== ----- COLLIDERS
        ///
        // ----- CIRCLE

        /// <summary>
        /// Draw a circle for a CircleCollider2D
        /// </summary>
        /// <param name="col"> CircleCollider2D drawn </param>
        /// <param name="color"> Color of the collider </param>
        public static void CircleCollider2D(CircleCollider2D col, Color color)
        {
            // Calculate the offset affected by scale
            Vector2 offset = col.offset;
            offset.x *= col.transform.lossyScale.x;
            offset.y *= col.transform.lossyScale.y;

            // Position of the drawing after rotation and offset
            Vector2 position = col.transform.position + col.transform.rotation * offset;

            // Draw
            DebugDraw2D.Circle2D(position, col.radius, color, col.transform.rotation, col.transform.lossyScale);
        }

        // ----- BOX / SQUARE

        /// <summary>
        /// Draw a circle for a BoxCollider2D
        /// </summary>
        /// <param name="col"> BoxCollider2D drawn </param>
        /// <param name="color"> Color of the collider </param>
        public static void BoxCollider2D(BoxCollider2D col, Color color)
        {
            // Calculate the offset affected by scale
            Vector2 offset = col.offset;
            offset.x *= col.transform.lossyScale.x;
            offset.y *= col.transform.lossyScale.y;

            // Position of the drawing after rotation and offset
            Vector2 position = col.transform.position + col.transform.rotation * offset;

            // Draw
            DebugDraw2D.Square2D(position, col.size, color, col.transform.rotation, col.transform.lossyScale);
        }

        // ----- CAPSULE

        /// <summary>
        /// Draw a circle for a CapsuleCollider2D
        /// </summary>
        /// <param name="col"> CapsuleCollider2D drawn </param>
        /// <param name="color"> Color of the collider </param>
        public static void CapsuleCollider2D(CapsuleCollider2D col, Color color)
        {
            // Calculate the offset affected by scale
            Vector2 offset = col.offset;
            offset.x *= col.transform.lossyScale.x;
            offset.y *= col.transform.lossyScale.y;

            // Position of the drawing after rotation and offset
            Vector2 position = col.transform.position + col.transform.rotation * offset;

            // Draw based on the direction of the capsule
            switch (col.direction)
            {
                case CapsuleDirection2D.Vertical:
                    DebugDraw2D.Capsule2D(position, col.size, color, AXIS.Y, col.transform.rotation, col.transform.lossyScale);
                    break;
                case CapsuleDirection2D.Horizontal:
                    DebugDraw2D.Capsule2D(position, col.size, color, AXIS.X, col.transform.rotation, col.transform.lossyScale);
                    break;
            }

        }

        // ----- POLYGON

        /// <summary>
        /// Draw a circle for a PolygonCollider2D
        /// </summary>
        /// <param name="col"> PolygonCollider2D drawn </param>
        /// <param name="color"> Color of the collider </param>
        public static void PolygonCollider2D(PolygonCollider2D col, Color color)
        {
            // Calculate the offset affected by scale
            Vector2 offset = col.offset;
            offset.x *= col.transform.lossyScale.x;
            offset.y *= col.transform.lossyScale.y;

            // Position of the drawing after rotation and offset
            Vector2 position = col.transform.position + col.transform.rotation * offset;

            // Draw
            DebugDraw2D.Polygon2D(position, col.points, color, col.transform.rotation, col.transform.lossyScale);
        }

        // ----- EDGE

        /// <summary>
        /// Draw a circle for a EdgeCollider2D
        /// </summary>
        /// <param name="col"> EdgeCollider2D drawn </param>
        /// <param name="color"> Color of the collider </param>
        public static void EdgeCollider2D(EdgeCollider2D col, Color color)
        {
            // Calculate the offset affected by scale
            Vector2 offset = col.offset;
            offset.x *= col.transform.lossyScale.x;
            offset.y *= col.transform.lossyScale.y;

            // Position of the drawing after rotation and offset
            Vector2 position = col.transform.position + col.transform.rotation * offset;

            // Draw
            DebugDraw2D.Edge2D(position, col.points, color, col.transform.rotation, col.transform.lossyScale);
        }

        // ===== ----- COLLIDER

        /// <summary>
        /// Draw any collider2D
        /// </summary>
        /// <param name="col"> Collider2D drawn </param>
        /// <param name="color"> Color of the Collider2D </param>
        public static void Collider2D(Collider2D col, Color color)
        {
            if (typeof(CircleCollider2D) == col.GetType())
            {
                DebugDraw2D.CircleCollider2D(col as CircleCollider2D, color);
            }
            else if (typeof(BoxCollider2D) == col.GetType())
            {
                DebugDraw2D.BoxCollider2D(col as BoxCollider2D, color);
            }
            else if (typeof(CapsuleCollider2D) == col.GetType())
            {
                DebugDraw2D.CapsuleCollider2D(col as CapsuleCollider2D, color);
            }
            else if (typeof(PolygonCollider2D) == col.GetType())
            {
                DebugDraw2D.PolygonCollider2D(col as PolygonCollider2D, color);
            }
            else if (typeof(EdgeCollider2D) == col.GetType())
            {
                DebugDraw2D.EdgeCollider2D(col as EdgeCollider2D, color);
            }
            else
                return;
        }
    }


}
