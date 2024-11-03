def generate_frame_data(start, end, prefix=""):
    result = []
    for n in range(start, end + 1):
        # Combina o prefixo com o número atual
        num = f"{prefix}{n}"
        result.append(f"new FrameData({num}, 0, 0, new List<GenericBox> {{}}),")
    return "\n".join(result)

# Exemplo de uso:
start = int(input("Digite o valor inicial: "))
end = int(input("Digite o valor final: "))
prefix = input("Digite o prefixo (opcional): ")

# Gera e imprime o resultado
output = generate_frame_data(start, end, prefix)
print(output)
input("-------------FIM-------------")
