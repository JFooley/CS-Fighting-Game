import os
import re
from PIL import Image, ImageTk
import tkinter as tk
from tkinter import filedialog

def extract_number_from_filename(filename):
    # Extrai o número do nome do arquivo usando regex
    match = re.search(r'\d+', filename)
    return match.group() if match else None

class FrameEditorApp:
    def __init__(self, root, files, scale=2, repetition=1):
        self.root = root
        self.files = files
        self.scale = scale
        self.repetition = repetition
        self.current_frame = 0
        self.hitboxes = []
        self.box_start = None
        self.frame_type = 1
        self.current_x = 0
        self.current_y = 0
        self.last_x = 0
        self.last_y = 0
        self.frame_data = []

        # Canvas e elementos UI
        self.window_width = 800
        self.window_height = 600
        self.canvas = tk.Canvas(root, width=self.window_width, height=self.window_height, bg="white")
        self.canvas.pack()
        self.box_type_label = tk.Label(root, text=f"Box Type: {self.frame_type}")
        self.box_type_label.pack()
        self.position_label = tk.Label(root, text=f"Movimento X: {self.current_x} Y: {self.current_y}")
        self.position_label.pack()

        # Create the grid background
        self.create_grid()

        # Indicate the origin point (0, 0)
        self.indicate_origin()

        # Bind eventos do mouse e teclado
        self.canvas.bind("<Button-1>", self.on_left_click)
        self.canvas.bind("<Button-3>", self.on_right_click)
        self.canvas.bind("<Motion>", self.on_mouse_move)
        self.root.bind("<BackSpace>", self.delete_last_box)
        self.root.bind("<Return>", self.next_frame)
        self.root.bind("<Left>", self.move_left)
        self.root.bind("<Right>", self.move_right)
        self.root.bind("<Up>", self.move_up)
        self.root.bind("<Down>", self.move_down)

        # Carregar primeira imagem
        self.load_next_image()

    def create_grid(self):
        # Draw a grid on the canvas
        for x in range(0, self.window_width, 100):
            self.canvas.create_line(x, 0, x, self.window_height, fill="black", width=2)
        for y in range(0, self.window_height, 100):
            self.canvas.create_line(0, y, self.window_width, y, fill="black", width=2)

        for x in range(0, self.window_width, 10):
            self.canvas.create_line(x, 0, x, self.window_height, fill="lightgrey")
        for y in range(0, self.window_height, 10):
            self.canvas.create_line(0, y, self.window_width, y, fill="lightgrey")

    def indicate_origin(self):
        origin_x = self.window_width // 2
        origin_y = self.window_height // 2
        self.canvas.create_line(origin_x - 5, origin_y, origin_x + 5, origin_y, fill="red")
        self.canvas.create_line(origin_x, origin_y - 5, origin_x, origin_y + 5, fill="red")

    def on_left_click(self, event):
        if not self.box_start:
            self.box_start = (event.x, event.y)
        else:
            x1, y1 = self.box_start
            x2, y2 = event.x, event.y
            self.hitboxes.append((x1, y1, x2, y2, self.frame_type))
            self.box_start = None
            self.canvas.create_rectangle(x1, y1, x2, y2, outline=self.get_box_color(self.frame_type))

    def on_right_click(self, event):
        self.frame_type = (self.frame_type + 1) % 3
        self.box_type_label.config(text=f"Box Type: {self.frame_type}")

    def on_mouse_move(self, event):
        if self.box_start:
            if hasattr(self, 'temp_rectangle'):
                self.canvas.delete(self.temp_rectangle)
            x1, y1 = self.box_start
            x2, y2 = event.x, event.y
            self.temp_rectangle = self.canvas.create_rectangle(x1, y1, x2, y2, outline=self.get_box_color(self.frame_type), dash=(2, 2))

    def delete_last_box(self, event):
        if self.hitboxes:
            self.hitboxes.pop()
            self.redraw()

    def next_frame(self, event):
        self.save_frame_data()
        self.current_frame += 1
        if self.current_frame < len(self.files):
            self.load_next_image()
        else:
            self.print_all_frames()
            self.root.quit()

    def load_next_image(self):
        self.canvas.delete("all")
        self.hitboxes.clear()
        self.current_x = self.current_y = 0
        self.last_x = self.last_y = 0
        self.image = Image.open(self.files[self.current_frame])
        new_size = (int(self.image.width * self.scale), int(self.image.height * self.scale))
        self.image = self.image.resize(new_size)
        self.photo = ImageTk.PhotoImage(self.image)
        self.canvas.config(width=self.image.width, height=self.image.height)
        self.canvas.create_image(0, 0, anchor=tk.NW, image=self.photo)
        self.redraw()

    def redraw(self):
        self.canvas.delete("all")
        self.canvas.create_image(0, 0, anchor=tk.NW, image=self.photo)
        for x1, y1, x2, y2, box_type in self.hitboxes:
            self.canvas.create_rectangle(x1, y1, x2, y2, outline=self.get_box_color(box_type))

    def get_box_color(self, box_type):
        return ["red", "blue", "white"][box_type]

    def move_left(self, event): self.current_x -= 1; self.update_offsets()
    def move_right(self, event): self.current_x += 1; self.update_offsets()
    def move_up(self, event): self.current_y -= 1; self.update_offsets()
    def move_down(self, event): self.current_y += 1; self.update_offsets()

    def update_offsets(self):
        self.position_label.config(text=f"Movimento X: {self.current_x} Y: {self.current_y}")

    def save_frame_data(self):
        frame_name = os.path.splitext(os.path.basename(self.files[self.current_frame]))[0]
        hitbox_str = ', '.join([f'new GenericBox({t}, {int(x1/self.scale)}, {int(y1/self.scale)}, {int(x2/self.scale)}, {int(y2/self.scale)})' for x1, y1, x2, y2, t in self.hitboxes])
        frame_data = f'new FrameData({frame_name}, {self.current_x/self.repetition}f, {self.current_y/self.repetition}f, new List<GenericBox> {{ {hitbox_str} }}),'
        self.frame_data.append(frame_data)

    def print_all_frames(self):
        print("\n".join(self.frame_data))

def main():
    root = tk.Tk()
    root.withdraw()  # Oculta a janela principal para abrir diálogo de seleção de arquivos
    files = filedialog.askopenfilenames(
        title="Selecione as imagens",
        filetypes=[("Imagens", "*.png;*.jpg;*.jpeg")]
    )
    if not files:
        print("Nenhum arquivo selecionado.")
        return
    repetition = int(input("Digite o framerate: "))

    app = FrameEditorApp(root, files, scale=2, repetition=60/repetition)
    root.deiconify()  # Mostra a janela principal do aplicativo
    root.mainloop()

if __name__ == "__main__":
    main()
