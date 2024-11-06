import os
import re
import tkinter as tk
from tkinter import filedialog

def extract_number_from_filename(filename):
    # Extrai o número do nome do arquivo usando regex
    match = re.search(r'\d+', filename)
    return match.group() if match else None

def generate_frame_data_from_files(files, prefix=""):
    result = []
    for file_path in files:
        # Extrai o nome do arquivo (sem extensão)
        filename = os.path.splitext(os.path.basename(file_path))[0]
        num = f"{prefix}{extract_number_from_filename(filename)}"
        result.append(f"new FrameData({num}, 0, 0, new List<GenericBox> {{}}),")
    return "\n".join(result)

# Abre uma janela de seleção de arquivos e gera o resultado
def main():
    # Configura a interface Tkinter para selecionar arquivos
    root = tk.Tk()
    root.withdraw()  # Esconde a janela principal

    # Define o diretório inicial como o diretório do script em execução
    initial_dir = os.path.dirname(os.path.abspath(__file__))

    # Abre o diálogo de seleção de arquivos
    files = filedialog.askopenfilenames(
        title="Selecione as imagens",
        filetypes=[("Imagens", "*.png;*.jpg;*.jpeg")],
        initialdir=initial_dir
    )
    if not files:
        print("Nenhum arquivo selecionado.")
        return

    # Pede o prefixo
    prefix = input("Digite o prefixo (opcional): ")

    # Gera o resultado
    output = generate_frame_data_from_files(files, prefix)

    # Salva o output em um arquivo "sprites.txt" no diretório do script
    output_path = os.path.join(initial_dir, "sprites.txt")
    with open(output_path, "w") as file:
        file.write(output)
    
    print(f"Arquivo salvo em: {output_path}")
    input("------------- FIM -------------")

if __name__ == "__main__":
    main()
