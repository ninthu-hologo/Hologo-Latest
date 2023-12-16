This pack contains a few shaders to create refractive Glass-like Materials.

Glass-like because the effect is a distortion based on surface Normal but not based
on a physical model of refraction!

To use a RefractionSurface shader
simply create a material and assign any of the shaders under "Refractive/" to it.

VERSION 1.3

For best results use: 
	Refractive/UnlitRefractionNEW or
	Refractive/LitRefractionNEW

these shaders use new method to calculate refraction.
This yields more accurate refraction and possibly better framerates.

Performance can be tuned with "Blur Samples" property.
Lower samples will add noise but is faster to render.

VERSION 1.2

There are a few options to optimise GPU load:
	LOW    : "Refractive/RefractSurf_noGloss_noMaps"
	MEDIUM : "Refractive/RefractSurf_noGloss"
	HIGH   : "Refractive/RefractSurf_full"
	
Keep in mind that these shaders make use of GrabPass to get texture of the scene behind
rendered object. So use it wisely!

for any questions write to: artkalev@gmail.com