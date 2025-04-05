#ifndef WORLDBEND_INCLUDED
#define WORLDBEND_INCLUDED

float3 BendWorldPos(float3 worldPos, float bendAmount)
{
    float radius = 1.0 / bendAmount;

    float angle = worldPos.z * bendAmount;
    float y = worldPos.y;
    float z = radius * sin(angle);
    y = y - radius * (1 - cos(angle));

    return float3(worldPos.x, y, z);
}

#endif
