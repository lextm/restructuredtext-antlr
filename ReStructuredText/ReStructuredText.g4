grammar ReStructuredText;

/* https://stackoverflow.com/questions/6178546/antlr-grammar-for-restructuredtext-rule-priorities */

options {
  language=CSharp;
}

parse
  :  (element | empty_line) + EOF
  ;

element
  :  section | sectionElement
  ;
  
sectionElement
  :  comment | listItemBullet | paragraph | listItemEnumerated | lineBlock
  ;

comment
  :  Comment line+
  ;

paragraph
  :  line+
  ;
  
section
  :  (Section LineBreak)? title Section (LineBreak)+ sectionElement*
  ;
  
lineBlock
  :  (Block line)+
  ;  

line
  :  indentation? text
  ;

empty_line
  :  Space* LineBreak
  ;

indentation
  :  Space+
  ;

text
  : text_fragment_start text_fragment* LineBreak
  ;
  
title
  : text_fragment+ LineBreak
  ;
 
text_fragment_start
  :  styledText
  |  interpretedText
  |  inlineLiteral
  |  reference
  |  hyperlinkTarget
  |  Section
  |  Star
  |  Plus
  |  Minus
  |  EscapeSequence
  |  Number
  |  Any
  ;

text_fragment
  :  text_fragment_start
  |  Space
  |  Block
  |  Bullet
  |  Literal
  |  Enumerated
  ;

styledText
  :  bold
  |  italic
  ;

bold
  :  Star Star boldAtom+ Star Star
  ;  

italic
  :  Star italicAtom+ (Star | StarSpace)
  ;

boldAtom
  :  ~Star | ~LineBreak
  |  italic
  ;

italicAtom
  :  ~Star | ~LineBreak
  |  bold
  ;

interpretedText
  :  BackTick interpretedTextAtoms BackTick
  ;

interpretedTextAtoms
  :  ~BackTick+
  ;

inlineLiteral
  :  BackTick BackTick inlineLiteralAtoms BackTick BackTick
  ;

inlineLiteralAtoms
  :  inlineLiteralAtom+
  ;

inlineLiteralAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

hyperlinkTarget
  :  UnderScore Any+
  ;
  
listItemBullet
  :  Bullet (paragraph+)?
  ;

listItemEnumerated
  :  Enumerated paragraph+ ending=LineBreak? 
  ;
  
Literal
  :  ':' LineBreak LineBreak* '::'
  ;

Section
  :  ('-' | '=' | '+') ('-' | '=' | '+')+
  ;
  
Bullet
  :  StarSpace 
  |  Plus Space 
  |  Minus Space
  ;
  
Enumerated
  :  (Number+ '.' Space)
  ;

StarSpace
  :  Star Space
  ;

Plus
  :  '+'
  ;

Minus
  :  '-'
  ;

Block
  :  '| '
  ;

Comment
  :  '..'
  ;

UnderScore
  :  '_'
  ;

BackTick
  :  '`'
  ;

Star
  :  '*'
  ;

Space
  :  ' ' 
  |  '\t'
  ;

EscapeSequence
  :  '\\' ('\\' | '*')
  ;

LineBreak
  :  '\r'? '\n'
  ;

Number
  : [0-9]
  ;

Any
  :  .
  ;