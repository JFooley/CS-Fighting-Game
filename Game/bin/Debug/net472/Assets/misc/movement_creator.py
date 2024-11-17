import os
import re
from PIL import Image, ImageTk
import tkinter as tk

class FrameMovementApp:
    def __init__(self, root, data, repeticao, scale=2):
        self.root = root
        self.data = data
        self.current_frame = 0
        self.scale = scale

        self.repeticao = repeticao;

        # Define the size of the window
        self.window_width = 800
        self.window_height = 600
        self.canvas = tk.Canvas(root, width=self.window_width, height=self.window_height, bg="white")
        self.canvas.pack()

        # Create the grid background
        self.create_grid()

        # Indicate the origin point (0, 0)
        self.indicate_origin()

        # Texto de posição
        self.Position_label = tk.Label(root, text=f"Movimento X: {self.data[self.current_frame][2]} Y: {self.data[self.current_frame][3]}")
        self.Position_label.pack()

        # Bind mouse and keyboard events
        self.root.bind("<Return>", self.next_frame)
        self.root.bind("<Left>", self.move_left)
        self.root.bind("<Right>", self.move_right)
        self.root.bind("<Up>", self.move_up)
        self.root.bind("<Down>", self.move_down)

        # Load the first image at the center of the window
        self.data[self.current_frame][2] = 0  # Centralizar em (0,0)
        self.data[self.current_frame][3] = 0  # Centralizar em (0,0)
        self.previous_image = None  # Store the previous image

        # Initialize current Position
        self.current_x = 0
        self.current_y = 0
        self.last_x = 0
        self.last_y = 0

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

    def redraw(self):
        self.canvas.delete("all")
        self.create_grid()
        self.indicate_origin()

        center_x = self.window_width // 2
        center_y = self.window_height // 2

        # Draw the previous image with 25% opacity if it exists
        if self.previous_image:
            self.draw_previous_image(center_x + self.last_x * self.scale, center_y + self.last_y * self.scale)

        # Draw the current image centered based on the current Position
        self.canvas.create_image(center_x + self.current_x * self.scale, 
                                  center_y + self.current_y * self.scale, 
                                  anchor=tk.CENTER, image=self.photo, tags="sprite")
        self.Position_label.config(text=f"Movimento X: {self.data[self.current_frame][2]} Y: {self.data[self.current_frame][3]}")

    def draw_previous_image(self, center_x, center_y):
        transparent_image = self.previous_image.copy()
        transparent_image.putalpha(64)  # 25% opacity
        transparent_photo = ImageTk.PhotoImage(transparent_image)

        self.canvas.create_image(center_x, center_y, anchor=tk.CENTER, image=transparent_photo)
        self.canvas.image = transparent_photo  # Keep a reference to avoid garbage collection

    def move_left(self, event):
        self.current_x -= 1
        self.update_offsets()
        self.redraw()

    def move_right(self, event):
        self.current_x += 1
        self.update_offsets()
        self.redraw()

    def move_up(self, event):
        self.current_y -= 1
        self.update_offsets()
        self.redraw()

    def move_down(self, event):
        self.current_y += 1
        self.update_offsets()
        self.redraw()

    def update_offsets(self):
        # Update the offsets based on the current and last Positions
        self.data[self.current_frame][2] = self.current_x - self.last_x
        self.data[self.current_frame][3] = self.current_y - self.last_y

    def next_frame(self, event):
        self.print_frame_data()
        self.current_frame += 1
        if self.current_frame < len(self.data):
            # Update the previous image reference and set the current Position
            self.previous_image = self.image if self.current_frame > 0 else None
            
            # Save the current Position as the last Position before loading the next image
            self.last_x = self.current_x
            self.last_y = self.current_y

            self.load_next_image()
        else:
            self.root.quit()

    def load_next_image(self):
        self.image = Image.open(self.data[self.current_frame][0])
        new_size = (int(self.image.width * self.scale), int(self.image.height * self.scale))
        self.image = self.image.resize(new_size)
        self.photo = ImageTk.PhotoImage(self.image)

        self.redraw()

    def print_frame_data(self):
        new_string = f"new FrameData({self.data[self.current_frame][1]}, {self.data[self.current_frame][2]/self.repeticao}f, {self.data[self.current_frame][3]/self.repeticao}f{self.data[self.current_frame][4]}),"
        print(new_string)

if __name__ == "__main__":
    file_name = "sprites.txt"

    if not os.path.exists(file_name):
        with open(file_name, 'w') as file:
            file.write("")

    with open(file_name, 'r') as file:
        linhas = file.readlines()

    rep = int(input("Digite o framerate: "))

    data = []
    pattern = r'new FrameData\((\d+),\s*(-?\d+),\s*(-?\d+)(.*)\)'

    for linha in linhas:
        match = re.search(pattern, linha)
        if match:
            img_name = int(match.group(1))
            x_offset = int(match.group(2))
            y_offset = int(match.group(3))
            string_sufix = match.group(4)

            data.append([f"{img_name}.png", img_name, x_offset, y_offset, string_sufix])

    root = tk.Tk()
    app = FrameMovementApp(root, data, 60/rep, scale=2)
    root.mainloop()
    input("------------- Finalizado -------------")
    input()
