using Engine.Source.Utility;
using Foster.Framework;


namespace Content.Assets.Shader;

public class CustomShader(GraphicsDevice graphicsDevice):
    Foster.Framework.Shader(graphicsDevice,GetCreateInfo(graphicsDevice),"Custom")
{
    private static ShaderCreateInfo GetCreateInfo(GraphicsDevice graphicsDevice) => new(
        Vertex:new(
            Code:EnginePlatform.ReadEmbeddedBytes<CustomShader>($"Custom.vertex.{graphicsDevice.Driver.GetShaderExtension()}"),
            SamplerCount:0,
            UniformBufferCount:1,
            EntryPoint:"vertex_main"
            ),
        Fragment:new(
            Code:EnginePlatform.ReadEmbeddedBytes<CustomShader>($"Custom.fragment.{graphicsDevice.Driver.GetShaderExtension()}"),
            SamplerCount:1,
            UniformBufferCount:0,
            EntryPoint:"fragment_main"
            )
        );
}