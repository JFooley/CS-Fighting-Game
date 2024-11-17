import os
from PIL import Image, ImageTk
import tkinter as tk
from tkinter import filedialog

class FrameMovementApp:
    def __init__(self, root, files, repeticao, scale=2):
        self.root = root
        self.files = files
        self.scale = scale

        self.end = False

        self.repeticao = repeticao

        self.window_width = 400 * scale
        self.window_height = 300 * scale
        self.canvas = tk.Canvas(root, width=self.window_width, height=self.window_height, bg="white")
        self.canvas.pack()

        # Dados
        self.current_frame = 0
        self.X = 0
        self.Y = 0
        self.current_x = self.window_width // 2 
        self.current_y = self.window_height // (3/2)
        self.last_x = 0
        self.last_y = 0

        self.box_type = 0
        self.hitboxes = []
        self.temp_rectangle = None
        self.box_start = None

        # Texto de posição
        self.Position_label = tk.Label(root, text=f"Movimento X: {self.X} Y: {self.Y}")
        self.box_type_label = tk.Label(root, text=f"Box Type: {self.box_type}")
        self.mouse_pos_label = tk.Label(root, text=f"{0},{0}")
        self.Position_label.pack()
        self.box_type_label.pack()
        self.mouse_pos_label.pack()

        self.canvas.focus_set()

        # Bind mouse and keyboard events
        self.canvas.bind("<Return>", self.next_frame)

        self.canvas.bind("<Left>", self.move_left)
        self.canvas.bind("<Right>", self.move_right)
        self.canvas.bind("<Up>", self.move_up)
        self.canvas.bind("<Down>", self.move_down)

        self.canvas.bind("<Button-1>", self.on_left_click)
        self.canvas.bind("<Button-3>", self.on_right_click)
        self.canvas.bind("<Motion>", self.on_mouse_move)
        self.canvas.bind("<BackSpace>", self.delete_last_box)

        # Load the first image at the center of the window
        self.current_image_string = None
        self.previous_image_string = None
        self.previus_photo = None
        self.current_photo = None

        self.load_next_image()

    def next_frame(self, event):
        if (not self.end): 
            self.save_data()
            self.current_frame += 1
            if self.current_frame < len(self.files):
                self.last_x = self.current_x + self.X
                self.last_y = self.current_y + self.Y
                self.current_x = self.last_x
                self.current_y = self.last_y
                self.X = 0
                self.Y = 0
                self.hitboxes.clear()

                self.load_next_image()
            else:
                self.end = True

        if (self.end):
            print("Terminou os frames")

    def load_next_image(self):
        # Load the next image and resize it
        self.current_image_string = self.files[self.current_frame]
        self.previous_image_string = self.files[self.current_frame - 1] if self.current_frame > 0 else None

        current_image = Image.open(self.current_image_string)
        current_image = current_image.resize((int(current_image.width * self.scale), int(current_image.height * self.scale)))
        self.current_photo = ImageTk.PhotoImage(current_image)

        if self.previous_image_string:
            previus_image = Image.open(self.previous_image_string)
            previus_image = previus_image.resize((int(previus_image.width * self.scale), int(previus_image.height * self.scale)))
            previus_image.putalpha(64)  # 25% opacity
            self.previus_photo = ImageTk.PhotoImage(previus_image)

        self.redraw()

    def redraw(self):
        self.canvas.delete("all")

        # Draw grid
        self.create_grid()

        # Draw the previous image with 25% opacity if it exists
        if self.previus_photo != None:
            self.canvas.create_image(self.last_x, self.last_y, anchor=tk.CENTER, image=self.previus_photo, tags="sprite")

        # Draw current image
        if self.current_photo != None:
            self.canvas.create_image((self.current_x + self.X), (self.current_y + self.Y), anchor=tk.CENTER, image=self.current_photo, tags="sprite")
        
        # Calcular a posição do canto superior esquerdo da imagem
        img_top_left_x = (self.current_x + self.X) - (self.current_photo.width() // 2)
        img_top_left_y = (self.current_y + self.Y) - (self.current_photo.height() // 2)

        # Draw hitboxes
        for x1, y1, x2, y2, box_type in self.hitboxes:
            self.canvas.create_rectangle(img_top_left_x + x1 * self.scale, img_top_left_y + y1 * self.scale, img_top_left_x + x2 * self.scale, img_top_left_y + y2 * self.scale, outline=self.get_box_color(box_type))

        # Draw labels
        self.Position_label.config(text=f"Movimento X: {self.X // self.scale} Y: {self.Y // self.scale}")
        self.box_type_label.config(text=f"Box Type: {self.box_type}")

    def create_grid(self):
        # Draw a grid on the canvas, scaled
        for x in range(0, self.window_width, 100 * self.scale):
            self.canvas.create_line(x, 0, x, self.window_height, fill="black", width=2)
        for y in range(0, self.window_height, 100 * self.scale):
            self.canvas.create_line(0, y, self.window_width, y, fill="black", width=2)

        for x in range(0, self.window_width, 10 * self.scale):
            self.canvas.create_line(x, 0, x, self.window_height, fill="lightgrey")
        for y in range(0, self.window_height, 10 * self.scale):
            self.canvas.create_line(0, y, self.window_width, y, fill="lightgrey")

        origin_x = self.window_width // 2
        origin_y = self.window_height // (3/2)
        self.canvas.create_line(origin_x - 5 * self.scale, origin_y, origin_x + 5 * self.scale, origin_y, fill="red")
        self.canvas.create_line(origin_x, origin_y - 5 * self.scale, origin_x, origin_y + 5 * self.scale, fill="red")

    def get_box_color(self, box_type):
        if box_type == 0:
            return "red"
        elif box_type == 1:
            return "blue"
        else:
            return "white"

    def save_data(self):
        frame_name = os.path.splitext(self.files[self.current_frame])[-1]
        hitbox_str = ', '.join([f'new GenericBox({t}, {int(x1)}, {int(y1)}, {int(x2)}, {int(y2)})' for x1, y1, x2, y2, t in self.hitboxes])
        print(f'new FrameData({frame_name}, {(self.X // self.scale) / self.repeticao}f, {(self.Y // self.scale) / self.repeticao}f, new List<GenericBox> {{ {hitbox_str} }}),')
    
    # Eventos
    def move_left(self, event):
        self.X -= 1 * self.scale
        if self.box_start: self.box_start[0] -= 1 * self.scale
        self.redraw()

    def move_right(self, event):
        self.X += 1 * self.scale
        if self.box_start: self.box_start[0] += 1 * self.scale
        self.redraw()

    def move_up(self, event):
        self.Y -= 1 * self.scale
        if self.box_start: self.box_start[1] -= 1 * self.scale
        self.redraw()

    def move_down(self, event):
        self.Y += 1 * self.scale
        if self.box_start: self.box_start[1] += 1 * self.scale
        self.redraw()

    def on_left_click(self, event):
        if not self.box_start:
            # Início da nova caixa
            self.box_start = [event.x, event.y]
        else:
            # Fim da caixa
            x1, y1 = self.box_start
            x2, y2 = event.x, event.y
            self.box_start = None
            
            # Desenhar a caixa no canvas com os pontos absolutos
            self.canvas.create_rectangle(x1, y1, x2, y2, outline=self.get_box_color(self.box_type))

            # Calcular a posição do canto superior esquerdo da imagem
            img_top_left_x = (self.current_x + self.X) - (self.current_photo.width() // 2)
            img_top_left_y = (self.current_y + self.Y) - (self.current_photo.height() // 2)

            # Ajuste para salvar as hitboxes com a posição relativa ao personagem
            adjusted_x1 = (x1 - img_top_left_x) // self.scale
            adjusted_y1 = (y1 - img_top_left_y) // self.scale
            adjusted_x2 = (x2 - img_top_left_x) // self.scale
            adjusted_y2 = (y2 - img_top_left_y) // self.scale
            
            # Adicionar a caixa à lista de hitboxes com a posição ajustada
            self.hitboxes.append((adjusted_x1, adjusted_y1, adjusted_x2, adjusted_y2, self.box_type))

    def on_right_click(self, event):
        # Cycle through box types
        self.box_type = (self.box_type + 1) % 3
        self.box_type_label.config(text=f"Box Type: {self.box_type}")

    def on_mouse_move(self, event):
        # Calcular a posição do canto superior esquerdo da imagem
        img_top_left_x = (self.current_x + self.X) - (self.current_photo.width() // 2)
        img_top_left_y = (self.current_y + self.Y) - (self.current_photo.height() // 2)

        self.mouse_pos_label.config(text=f"{(event.x - img_top_left_x) // self.scale} - {(event.y - img_top_left_y) // self.scale}")

        if self.box_start:
            if self.temp_rectangle:
                self.canvas.delete(self.temp_rectangle)
            x1, y1 = self.box_start
            x2, y2 = event.x, event.y
            self.temp_rectangle = self.canvas.create_rectangle(x1, y1, x2, y2, outline=self.get_box_color(self.box_type), dash=(2, 2))

    def delete_last_box(self, event):
        if self.hitboxes:
            self.hitboxes.pop()
            self.redraw()

if __name__ == "__main__":

    initial_dir = os.path.dirname(os.path.abspath(__file__))

    rep = int(input("Digite o framerate: "))

    # Abre o diálogo de seleção de arquivos
    files = filedialog.askopenfilenames(
        title="Selecione as imagens",
        filetypes=[("Imagens", "*.png;*.jpg;*.jpeg")],
        initialdir=initial_dir
    )
    if not files:
        print("Nenhum arquivo selecionado.")

    root = tk.Tk()
    app = FrameMovementApp(root, files, 60/rep, scale=2)
    root.mainloop()
    input("------------- Finalizado -------------")
    input()
