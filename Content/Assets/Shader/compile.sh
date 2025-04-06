compile() {
	local input=$1
	local stage=$2
	local outdir=$3
	#输入example.hlsl，stage为vertex,生成example.vertex
	local filename="$(basename -- "$input")"
	local filename="${filename%.*}.$stage"
	echo "Compiling '$1' $stage stage ..."
	#-e"{stage}_main"指定入口函数为vertex_main或者fragment_main
	#-t$stage:指定着色器阶段
	#-s HLSL 输入语言为HLSL
	# -o 输出SPIR-V文件，example.vertex.spv
	shadercross "$input" -e "${stage}_main" -t $stage -s HLSL -o "$outdir/$filename.spv"
	#从spv生成metal shadering language
	shadercross "$outdir/$filename.spv" -e "${stage}_main" -t $stage -s SPIRV -o "$outdir/$filename.msl"
	#从spv生成DXIL
	shadercross "$outdir/$filename.spv" -e "${stage}_main" -t $stage -s SPIRV -o "$outdir/$filename.dxil"
}
#获取脚本所在目录的绝对路径，确保后续操作不受执行路径影响
#遍历脚本目录下所有 .hlsl 文件。对每个文件分别编译其 ​​顶点着色器（vertex）​​ 和 ​​片段着色器（fragment）​​。输出到 Compiled 子目录中。
SCRIPT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )

for file in $SCRIPT_DIR/*.hlsl; do
	compile "$file" vertex $SCRIPT_DIR/Compiled
	compile "$file" fragment $SCRIPT_DIR/Compiled
done