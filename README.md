Projeto de Fighting Game feito em C# usando SFML 

Implementado:
- Sistema de input segregrado para player 1 e 2
- Sistema de detecção de motion inputs (meia lua, etc)
- Sistema de som
- Animações
- Camera dinâmica
- Janela de tamanho dinamico
- Sistema genérico personalizável de estados
- Stages animados com behaviour próprio
- Hitboxes e sistema para criar hitboxes
- Colisão
- Sistema para inferir estado no oponente
- Cancels
- Block
- Auto orientação da direção lógica dos personagens
- Pushback em golpes
- Lógica básica de combate (tempo, rounds, gatilhos de fim de round, resets)
- Orientação visual dos personagens (olharem para o lado correto)
- Padronização dos controles
- Hitstop
- Pushback usando as pushboxes
- Particulas de dano e block
- HUD de Batalha
- Estados de intervalo entre rounds
- Dano e stun na colisão
- 2° controle conectado se torna o player 2 e o teclado fica como default
- Pushback no corner: o personagem que está batendo receber o pushback pois o que está sendo atingido não pode se afastar mais devido ao corner
- Capacidade de realizar Super Arts
- Hitstop melhorado
- Multiplos hits por animação (com reinício definido no frame)
- Fixed: Jogadores não podem se afastar um do outro mais do que o tamanho da tela.
- Particulas visuais independentes (Super Art, etc)
- Hitstun e blockstun
- Combo e indicação individual
- Damage scaling
- Física complexa
- Modo treino
- Menu de pausa
- Lógica do jogo mais robusta
- Menu de seleção de stage e personagem
- Tela pós batalha com rematch instantâneo
- Tela de configurações com permanência dos dados
- Sistema de prioridade de golpes
- Layout de teclado adaptável para P1 e P2 (possibilita jogar com uma pessoa só)
- Serialização dos dados (poupa tempo de load)
- Imposição de movimento unificada (Agora usa apenas o método Push)
- Trail visual para Super e movimentos EX
- Sistema de mudança sutil na cor da sprite de acordo com cada stage (simula uma iluminação global simples)

Deprecated:
- Física simples

Em desenvolvimento:
- Feat: Mecanica de stun quando a barra de stun fica cheia
- Feat: Limite de juggle
- Feat: Uso de 3 hitboxes (high, medium, low) para definir animações de hit diferentes e posição da hitspark
