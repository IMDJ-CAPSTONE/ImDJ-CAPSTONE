/*! @file       : 	CubesCreator.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	These files are unused
*/

using UnityEngine;

/*! <summary>
*  These files are unused
*  </summary>
*/
public class CubesCreator : MonoBehaviour
{
    public GameObject prefab;
    private const int N = 5;

    // Use this for initialization
    private void Start()
    {
        for (int z = -N; z <= N; z++)
        {
            for (int y = -N; y <= N; y++)
            {
                for (int x = -N; x <= N; x++)
                {
                    Instantiate(prefab, new Vector3(x * 1.2f, y * 1.2f, z * 1.2f), Quaternion.identity, transform);
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
