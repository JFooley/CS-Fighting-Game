import os
from PIL import Image, ImageTk
import tkinter as tk

class FrameDataApp:
    def __init__(self, root, image_files, scale=2):
        self.root = root
        self.image_files = image_files
        self.frame_type = 1
        self.current_frame = 0
        self.hitboxes = []
        self.box_start = None
        self.scale = scale
        self.temp_rectangle = None

        # Create a canvas to display the image
        self.canvas = tk.Canvas(root)
        self.canvas.pack()

        # Label to show the current box type
        self.box_type_label = tk.Label(root, text=f"Box Type: {self.frame_type}")
        self.box_type_label.pack()


        # Bind mouse and keyboard events
        self.canvas.bind("<Button-1>", self.on_left_click)
        self.canvas.bind("<Button-3>", self.on_right_click)
        self.canvas.bind("<Motion>", self.on_mouse_move)
        self.root.bind("<BackSpace>", self.delete_last_box)
        self.root.bind("<Return>", self.next_frame)
        self.root.bind("<space>", self.show_previous_frame)
        self.root.bind("<KeyRelease-space>", self.show_current_frame)

        # Load the first image
        self.load_next_image()

    def on_left_click(self, event):
        if not self.box_start:
            # Start of new box
            self.box_start = (event.x, event.y)
        else:
            # End of box
            x1, y1 = self.box_start
            x2, y2 = event.x, event.y
            self.hitboxes.append((x1, y1, x2, y2, self.frame_type))
            self.box_start = None
            self.canvas.create_rectangle(x1, y1, x2, y2, outline=self.get_box_color(self.frame_type))

    def on_right_click(self, event):
        # Cycle through box types
        self.frame_type = (self.frame_type + 1) % 3
        self.box_type_label.config(text=f"Box Type: {self.frame_type}")

    def on_mouse_move(self, event):
        if self.box_start:
            if self.temp_rectangle:
                self.canvas.delete(self.temp_rectangle)
            x1, y1 = self.box_start
            x2, y2 = event.x, event.y
            self.temp_rectangle = self.canvas.create_rectangle(x1, y1, x2, y2, outline=self.get_box_color(self.frame_type), dash=(2, 2))

    def delete_last_box(self, event):
        if self.hitboxes:
            self.hitboxes.pop()
            self.redraw()

    def next_frame(self, event):
        self.print_frame_data()
        self.current_frame += 1
        if self.current_frame < len(self.image_files):
            self.load_next_image()
        else:
            self.root.quit()

    def load_next_image(self):
        # Clear the canvas
        self.canvas.delete("all")
        self.hitboxes.clear()

        # Load the next image and resize it
        self.image = Image.open(self.image_files[self.current_frame])
        new_size = (int(self.image.width * self.scale), int(self.image.height * self.scale))
        self.image = self.image.resize(new_size)
        self.photo = ImageTk.PhotoImage(self.image)
        self.canvas.config(width=self.image.width, height=self.image.height)
        self.canvas.create_image(0, 0, anchor=tk.NW, image=self.photo)

        # Redraw hitboxes if any
        self.redraw()

    def redraw(self):
        self.canvas.delete("all")
        self.canvas.create_image(0, 0, anchor=tk.NW, image=self.photo)
        for x1, y1, x2, y2, box_type in self.hitboxes:
            self.canvas.create_rectangle(x1, y1, x2, y2, outline=self.get_box_color(box_type))

    def get_box_color(self, box_type):
        if box_type == 0:
            return "red"
        elif box_type == 1:
            return "blue"
        else:
            return "white"

    def show_previous_frame(self, event):
        if self.current_frame > 0:
            # Save current hitboxes
            current_hitboxes = self.hitboxes[:]
            # Load previous image and hitboxes
            self.current_frame -= 1
            self.load_next_image()
            # Restore the current state
            self.current_frame += 1
            self.hitboxes = current_hitboxes
        self.canvas.delete("all")
        self.canvas.create_image(0, 0, anchor=tk.NW, image=self.photo)

    def show_current_frame(self, event):
        self.redraw()

    def print_frame_data(self):
        frame_name = os.path.splitext(os.path.basename(self.image_files[self.current_frame]))[0]
        hitbox_str = ', '.join([f'new GenericBox({t}, {int(x1/self.scale)}, {int(y1/self.scale)}, {int(x2/self.scale)}, {int(y2/self.scale)})' for x1, y1, x2, y2, t in self.hitboxes])
        print(f'new FrameData({frame_name}, 0, 0, new List<GenericBox> {{ {hitbox_str} }}),')

if __name__ == "__main__":
    # Define the image filenames here
    frame_numbers = input("Frame index list: ")
    image_files = [f"{num}.png" for num in frame_numbers.split(", ")]

    root = tk.Tk()
    app = FrameDataApp(root, image_files, scale=2)
    root.mainloop()
    input("------------- Finalizado -------------")
