using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Random = Unity.Mathematics.Random;

namespace EG_MaterialProperty
{
    public partial class ChangeColorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var random = new Random(1234);
            Entities.ForEach((ref URPMaterialPropertyBaseColor color) =>
                {
                    color.Value = new float4(
                        random.NextFloat(1.0f),
                        .0f,
                        .0f,
                        1.0f);
                })
                .Schedule();
        }
    }
}