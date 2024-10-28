Projeto de Fighting Game feito em C# usando SFML 

Implementado:
- Sistema de input segregrado para player 1 e 2
- Sistema de detecção de motion inputs (meia lua, etc)
- Sistema de som
- Animações
- Camera dinâmica
- Janela de tamanho dinamico
- Física simples
- Sistema genérico personalizável de estados
- Stages animados com behaviour próprio
- Hitboxes e sistema para criar hitboxes
- Colisão
- Sistema para inferir estado no oponente
- Cancels
- Block
- Dash universal
- Auto orientação da direção lógica dos personagens
- Pushback em golpes
- Lógica básica de combate (tempo, rounds, gatilhos de fim de round, resets)
- Orientação visual dos personagens (olharem para o lado correto)
- Padronização dos controles
- Hitstop
- Pushback usando as pushboxes
  
Em desenvolvimento:
- HUD
- Dano e stun na colisão
- Lógica do jogo mais robusta
- Estados de intervalo entre rounds
- Menu de seleção de personagem e stage

Para rodar execute dotnet run no terminal

Controles
Teclado:
Setas - Movimentação
Q = A 	- Soco Fraco
W = B 	- Soco Forte
A = C 	- Chute Fraco
S = D 	- Chute Forte
E = RB 	- (depende do personagem)
D = RT 	- (depende do personagem)
R = LB	- (depende do personagem)
F = LT	- (depende do personagem)
Enter = Start - Mostra as hitboxs

Controle:
Setas - Movimentação
A = A   - Soco Fraco
B = B   - Soco Forte
X = C   - Chute Fraco
Y = D   - Chute Forte
RB = RB - (depende do personagem)
RT = RT - (depende do personagem)
LB = LB - (depende do personagem)
LT = LT - (depende do personagem)
Start = Start - Mostra as hitboxs
